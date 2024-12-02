public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IWebHostEnvironment _env;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        ProblemDetails problemDetails;

        if (exception is FluentValidation.ValidationException validationException)
        {
            problemDetails = HandleValidationException(context, validationException);
        }
        else
        {
            var statusCode = GetStatusCodeError(exception);
            problemDetails = CreateProblemDetails(context, exception, statusCode);
        }

        // Log da exceção
        _logger.LogError(exception, problemDetails.Detail);

        context.Response.StatusCode = problemDetails.Status ?? 500;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
        return true; // Indica que a exceção foi tratada
    }

    private ProblemDetails HandleValidationException(HttpContext context, FluentValidation.ValidationException exception)
    {
        var errors = exception.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        return new ValidationProblemDetails(errors)
        {
            Title = "Erro de validação",
            Detail = "Um ou mais erros de validação ocorreram.",
            Status = StatusCodes.Status400BadRequest,
            Instance = context.Request.Path
        };
    }

    private ProblemDetails CreateProblemDetails(HttpContext context, Exception exception, HttpStatusCode statusCode)
    {
        var problemDetails = new ProblemDetails
        {
            Title = GetTitle(statusCode),
            Detail = GetDetail(exception),
            Status = (int)statusCode,
            Instance = context.Request.Path
        };

        if (!_env.IsProduction())
        {
            problemDetails.Extensions["stackTrace"] = exception.StackTrace;
        }

        return problemDetails;
    }

    private static string GetTitle(HttpStatusCode statusCode) =>
        statusCode switch
        {
            HttpStatusCode.BadRequest => "Requisição inválida",
            HttpStatusCode.Unauthorized => "Acesso negado",
            HttpStatusCode.Forbidden => "Permissão negada",
            HttpStatusCode.NotFound => "Recurso não encontrado",
            _ => "Erro interno do servidor"
        };

    private string GetDetail(Exception exception) =>
        exception switch
        {
            AuthenticationException => "Você precisa autenticar para acessar este recurso.",
            UnauthorizedAccessException => "Você não tem permissão para acessar este recurso.",
            _ => "Ocorreu um erro ao processar sua solicitação."
        };

    private static HttpStatusCode GetStatusCodeError(Exception exception) =>
        exception switch
        {
            ValidationException => HttpStatusCode.BadRequest,
            AuthenticationException => HttpStatusCode.Unauthorized,
            ArgumentException => HttpStatusCode.BadRequest,
            KeyNotFoundException => HttpStatusCode.NotFound,
            _ => HttpStatusCode.InternalServerError
        };
}





var builder = WebApplication.CreateBuilder(args);

// Registrar o handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Adicionar ProblemDetails
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = ctx =>
    {
        ctx.ProblemDetails.Extensions["traceId"] = ctx.HttpContext.TraceIdentifier;
    };
});

var app = builder.Build();

// Configurar o middleware de exceção global
app.UseExceptionHandler();

// Exemplo de rota com validação
app.MapPost("/example", (MyModel model, IValidator<MyModel> validator) =>
{
    var validationResult = validator.Validate(model);
    if (!validationResult.IsValid)
    {
        throw new ValidationException(validationResult.Errors);
    }

    return Results.Ok("Validação bem-sucedida!");
});

app.Run();








public class NotFoundToNullHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = await base.SendAsync(request, cancellationToken);

        // Retornar uma resposta de sucesso simulada se for 404
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            // Criar uma resposta vazia com status de sucesso para que o Refit trate como `null`
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(string.Empty), // Evitar erros de deserialização
                RequestMessage = request
            };
        }

        return response;
    }
}
