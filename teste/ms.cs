using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

public class ContractValidationMiddleware
{
    private readonly RequestDelegate _next;
    // Cache para armazenar os metadados das rotas (rota normalizada -> informações da rota)
    private static readonly ConcurrentDictionary<string, RouteInfo> RouteCache =
        new ConcurrentDictionary<string, RouteInfo>(StringComparer.OrdinalIgnoreCase);

    // Prefixo do middleware (pode ser parametrizado via options se necessário)
    private const string MiddlewarePrefix = "/contract-validation";

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
                // Assegura que o cache de rotas está construído
                if (RouteCache.IsEmpty)
                {
                    BuildRouteCache();
                }

                // Remove o prefixo do middleware e normaliza a rota solicitada
                string requestedRoute = NormalizePath(requestPath.Substring(MiddlewarePrefix.Length));

                // Caso a rota esteja vazia, define como raiz "/"
                if (string.IsNullOrWhiteSpace(requestedRoute))
                {
                    requestedRoute = "/";
                }

                if (RouteCache.TryGetValue(requestedRoute, out RouteInfo? routeInfo))
                {
                    // Verifica se o método HTTP da requisição é permitido para esta rota
                    if (routeInfo.HttpMethods.Any() &&
                        !routeInfo.HttpMethods.Contains(context.Request.Method, StringComparer.OrdinalIgnoreCase))
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
                    object? fakeResponse = GenerateMockResponse(routeInfo.ReturnType);

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
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        message = "Rota não encontrada"
                    }));
                    return;
                }
            }

            // Caso não seja uma rota para contract-validation, passa para o próximo middleware
            await _next(context);
        }
        catch (Exception ex)
        {
            // Em caso de exceção, retorna um erro 500
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            // Atenção: em produção evite expor detalhes da exceção
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                message = "Erro interno do servidor",
                error = ex.Message
            }));
        }
    }

    /// <summary>
    /// Constroi o cache de rotas, iterando pelos controllers e seus métodos.
    /// </summary>
    private static void BuildRouteCache()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        var controllers = assembly.GetTypes()
            .Where(t => typeof(ControllerBase).IsAssignableFrom(t) && !t.IsAbstract);

        foreach (var controller in controllers)
        {
            // Obtém a rota definida no controller ou utiliza o padrão "api/NomeController"
            var routeAttribute = controller.GetCustomAttribute<RouteAttribute>();
            string controllerRoute = routeAttribute?.Template ?? $"api/{controller.Name.Replace("Controller", "")}";

            // Normaliza a rota do controller
            string normalizedControllerRoute = NormalizePath(controllerRoute);

            // Obtém os métodos públicos declarados no controller
            var methods = controller.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach (var method in methods)
            {
                // Obtém os atributos HTTP (HttpGet, HttpPost, etc.)
                var httpAttributes = method.GetCustomAttributes<HttpMethodAttribute>().ToList();
                if (httpAttributes.Any())
                {
                    foreach (var httpAttr in httpAttributes)
                    {
                        // Lista de métodos HTTP permitidos para esta ação
                        List<string> allowedMethods = httpAttr.HttpMethods?.ToList() ?? new List<string>();

                        // Obtém a rota do método (ação)
                        string methodRoute = httpAttr.Template ?? "";
                        string fullRoute = CombineRoutes(normalizedControllerRoute, methodRoute);

                        // Adiciona ou atualiza o cache de rotas
                        RouteCache.AddOrUpdate(fullRoute,
                            key => new RouteInfo
                            {
                                ReturnType = method.ReturnType,
                                HttpMethods = allowedMethods
                            },
                            (key, existingRoute) =>
                            {
                                // Mescla os métodos HTTP permitidos
                                foreach (var m in allowedMethods)
                                {
                                    if (!existingRoute.HttpMethods.Contains(m, StringComparer.OrdinalIgnoreCase))
                                        existingRoute.HttpMethods.Add(m);
                                }
                                return existingRoute;
                            });
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
    /// Normaliza uma rota: remove espaços, adiciona barra inicial, remove barra final e converte para minúsculas.
    /// </summary>
    private static string NormalizePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return "/";

        string trimmed = path.Trim().Trim('/');
        return "/" + trimmed.ToLowerInvariant();
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

            // Trata de ActionResult<T> e Task<T>
            if (returnType.IsGenericType)
            {
                Type genericDefinition = returnType.GetGenericTypeDefinition();
                if (genericDefinition == typeof(ActionResult<>))
                    return GenerateMockResponse(returnType.GetGenericArguments()[0]);

                if (genericDefinition == typeof(Task<>))
                    return GenerateMockResponse(returnType.GetGenericArguments()[0]);
            }

            // Para tipos complexos, tenta criar uma instância via Activator
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
    /// Classe interna para armazenar informações sobre uma rota.
    /// </summary>
    private class RouteInfo
    {
        public Type ReturnType { get; set; }
        public List<string> HttpMethods { get; set; } = new List<string>();
    }
}
