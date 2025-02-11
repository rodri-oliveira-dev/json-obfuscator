using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

public class ContractValidationMiddleware
{
    private readonly RequestDelegate _next;
    private const string MiddlewarePrefix = "/contract-validation";
    // Cache de rotas armazenado como lista de RouteInfo
    private static readonly List<RouteInfo> RouteCache = new List<RouteInfo>();
    private static readonly object CacheLock = new object();

    public ContractValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            string? requestPath = context.Request.Path.Value;
            if (!string.IsNullOrEmpty(requestPath) &&
                requestPath.StartsWith(MiddlewarePrefix, StringComparison.OrdinalIgnoreCase))
            {
                // Constrói o cache de rotas se ainda não foi feito
                if (!RouteCache.Any())
                {
                    lock (CacheLock)
                    {
                        if (!RouteCache.Any())
                        {
                            BuildRouteCache();
                        }
                    }
                }

                // Remove o prefixo do middleware e normaliza a rota solicitada
                string requestedRoute = NormalizePath(requestPath.Substring(MiddlewarePrefix.Length));
                if (string.IsNullOrWhiteSpace(requestedRoute))
                {
                    requestedRoute = "/";
                }

                // Procura uma rota mapeada que corresponda
                RouteInfo? matchedRoute = RouteCache.FirstOrDefault(ri =>
                    string.Equals(ri.Template, requestedRoute, StringComparison.OrdinalIgnoreCase) ||
                    (ri.TemplateRegex != null && ri.TemplateRegex.IsMatch(requestedRoute))
                );

                if (matchedRoute != null)
                {
                    // Verifica se o método HTTP é permitido para esta rota
                    if (matchedRoute.HttpMethods.Any() &&
                        !matchedRoute.HttpMethods.Contains(context.Request.Method, StringComparer.OrdinalIgnoreCase))
                    {
                        context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            message = "Método HTTP não permitido para esta rota"
                        }));
                        return;
                    }

                    // Gera a resposta simulada com base no tipo de retorno da ação
                    object? fakeResponse = GenerateMockResponse(matchedRoute.ReturnType);
                    context.Response.ContentType = "application/json";
                    var responsePayload = new
                    {
                        data = fakeResponse,
                        queryParameters = context.Request.Query
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(responsePayload));
                    return;
                }
                else
                {
                    // Rota não encontrada
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new { message = "Rota não encontrada" }));
                    return;
                }
            }

            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                message = "Erro interno do servidor",
                error = ex.Message
            }));
        }
    }

    /// <summary>
    /// Itera pelos controllers e ações para construir o cache de rotas.
    /// </summary>
    private static void BuildRouteCache()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        var controllers = assembly.GetTypes()
            .Where(t => typeof(ControllerBase).IsAssignableFrom(t) && !t.IsAbstract);

        foreach (var controller in controllers)
        {
            // Obtém a rota do controller ou utiliza o padrão "api/NomeController"
            var routeAttribute = controller.GetCustomAttribute<RouteAttribute>();
            string controllerRoute = routeAttribute?.Template ?? $"api/{controller.Name.Replace("Controller", "")}";
            string normalizedControllerRoute = NormalizePath(controllerRoute);

            var methods = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach (var method in methods)
            {
                // Obtém os atributos HTTP (por exemplo, HttpGet, HttpPost)
                var httpAttributes = method.GetCustomAttributes<HttpMethodAttribute>().ToList();
                if (httpAttributes.Any())
                {
                    foreach (var httpAttr in httpAttributes)
                    {
                        List<string> allowedMethods = httpAttr.HttpMethods?.ToList() ?? new List<string>();

                        string methodRoute = httpAttr.Template ?? "";
                        string fullRoute = CombineRoutes(normalizedControllerRoute, methodRoute);

                        // Verifica se já existe um mapeamento para o mesmo template
                        var existing = RouteCache.FirstOrDefault(ri =>
                            string.Equals(ri.Template, fullRoute, StringComparison.OrdinalIgnoreCase));

                        if (existing != null)
                        {
                            foreach (var m in allowedMethods)
                            {
                                if (!existing.HttpMethods.Contains(m, StringComparer.OrdinalIgnoreCase))
                                    existing.HttpMethods.Add(m);
                            }
                        }
                        else
                        {
                            // Se o template contém parâmetros (ex: {documentNumber}), gera uma expressão regular para match
                            Regex? regex = BuildRegexForRoute(fullRoute);
                            RouteCache.Add(new RouteInfo
                            {
                                Template = fullRoute,
                                TemplateRegex = regex,
                                ReturnType = method.ReturnType,
                                HttpMethods = allowedMethods
                            });
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Combina a rota do controller com a rota do método e normaliza o resultado.
    /// </summary>
    private static string CombineRoutes(string controllerRoute, string methodRoute)
    {
        if (string.IsNullOrEmpty(methodRoute))
        {
            return controllerRoute;
        }
        string combined = $"{controllerRoute}/{methodRoute}";
        return NormalizePath(combined);
    }

    /// <summary>
    /// Normaliza uma rota: remove barras desnecessárias, converte para minúsculas e garante a barra inicial.
    /// </summary>
    private static string NormalizePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return "/";
        string trimmed = path.Trim().Trim('/');
        return "/" + trimmed.ToLowerInvariant();
    }

    /// <summary>
    /// Se a rota contiver parâmetros (ex: {documentNumber}), converte o template em uma expressão regular.
    /// </summary>
    private static Regex? BuildRegexForRoute(string route)
    {
        if (!route.Contains("{"))
        {
            return null;
        }
        // Converte cada parâmetro para um grupo que capture qualquer sequência de caracteres exceto '/'
        string pattern = "^" + Regex.Replace(route, @"\{[^/]+\}", "([^/]+)") + "$";
        return new Regex(pattern, RegexOptions.IgnoreCase);
    }

    /// <summary>
    /// Gera uma resposta simulada (mock) com base no tipo de retorno.
    /// </summary>
    private object? GenerateMockResponse(Type returnType)
    {
        try
        {
            if (returnType == typeof(string))
                return "Teste de resposta simulada";
            if (returnType == typeof(int) || returnType == typeof(long) ||
                returnType == typeof(float) || returnType == typeof(double))
                return 123;
            if (returnType == typeof(bool))
                return true;
            if (returnType == typeof(void) || returnType == typeof(Task))
                return new { message = "Nenhum conteúdo" };
            if (returnType.IsGenericType)
            {
                Type genericDefinition = returnType.GetGenericTypeDefinition();
                if (genericDefinition == typeof(ActionResult<>))
                    return GenerateMockResponse(returnType.GetGenericArguments()[0]);
                if (genericDefinition == typeof(Task<>))
                    return GenerateMockResponse(returnType.GetGenericArguments()[0]);
            }
            if (returnType.IsClass)
            {
                try
                {
                    return Activator.CreateInstance(returnType);
                }
                catch
                {
                    return new { message = "Não foi possível criar instância do tipo" };
                }
            }
            return new { message = "Tipo não reconhecido" };
        }
        catch (Exception ex)
        {
            return new { message = "Erro ao gerar resposta simulada", error = ex.Message };
        }
    }

    /// <summary>
    /// Representa as informações mapeadas para uma rota.
    /// </summary>
    private class RouteInfo
    {
        // Template normalizado (ex: "/canauthorize/{documentnumber}")
        public string Template { get; set; } = "/";
        // Se a rota contém parâmetros, essa propriedade conterá uma Regex para fazer a correspondência
        public Regex? TemplateRegex { get; set; }
        public Type ReturnType { get; set; } = typeof(void);
        public List<string> HttpMethods { get; set; } = new List<string>();
    }
}