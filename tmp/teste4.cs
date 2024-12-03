public class CustomJsonContentSerializer : IContentSerializer
{
    private readonly JsonSerializerOptions _jsonOptions;

    public CustomJsonContentSerializer()
    {
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    public async Task<T?> DeserializeAsync<T>(HttpContent content)
    {
        // Se não houver conteúdo, retorna null
        if (content == null || content.Headers.ContentLength == 0)
            return default;

        var stream = await content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<T>(stream, _jsonOptions);
    }

    public Task SerializeAsync<T>(HttpContent content, T value)
    {
        throw new NotImplementedException("Serialization is not required in this example.");
    }
}


public static void ConfigureRefit(this IServiceCollection services, IConfiguration configuration)
{
    services
        .AddRefitClient<IExternalService>(new RefitSettings
        {
            ContentSerializer = new CustomJsonContentSerializer()
        })
        .ConfigureHttpClient(c => c.BaseAddress = 
            new Uri(configuration["ExternalService:Url"]));
}
