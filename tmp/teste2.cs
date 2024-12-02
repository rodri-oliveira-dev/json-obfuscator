public class LoggingHandler : DelegatingHandler
{
    private readonly ILogger<LoggingHandler> _logger;

    public LoggingHandler(ILogger<LoggingHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Log antes de enviar a requisição
        _logger.LogInformation("Enviando requisição para {Url} com método {Method}", request.RequestUri, request.Method);

        if (request.Content != null)
        {
            var content = await request.Content.ReadAsStringAsync();
            _logger.LogInformation("Conteúdo da requisição: {Content}", content);
        }

        // Enviar a requisição
        var response = await base.SendAsync(request, cancellationToken);

        // Log após receber a resposta
        _logger.LogInformation("Recebida resposta com status {StatusCode} de {Url}", response.StatusCode, response.RequestMessage?.RequestUri);

        if (response.Content != null)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            _logger.LogInformation("Conteúdo da resposta: {ResponseContent}", responseContent);
        }

        return response;
    }
}



public static void ConfigureRefit(this IServiceCollection services, IConfiguration configuration)
{
    services
        .AddRefitClient<IExternalService>()
        .ConfigureHttpClient(c => c.BaseAddress = 
            new Uri(configuration.GetSection("ExternalService:Url").Value!))
        .AddHttpMessageHandler<LoggingHandler>(); // Adiciona o handler de logging

    // Registra o handler no DI
    services.AddTransient<LoggingHandler>();
}
