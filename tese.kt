import io.micronaut.context.annotation.Factory
import io.micronaut.http.client.HttpClientConfiguration
import io.micronaut.http.client.HttpClientFactory
import io.micronaut.http.client.HttpClientOptions
import io.micronaut.runtime.ApplicationConfiguration

@Factory
class CustomHttpClientFactory(
    private val applicationConfiguration: ApplicationConfiguration,
    private val httpClientConfiguration: HttpClientConfiguration
) {

    @Singleton
    fun customHttpClient(
        @Named("custom") httpClientOptions: HttpClientOptions
    ): HttpClient {
        return HttpClientFactory.create(httpClientOptions, applicationConfiguration, httpClientConfiguration)
    }

    @Named("custom")
    @Singleton
    fun customHttpClientOptions(): HttpClientOptions {
        return HttpClientOptions()
            .apply {
                propagation = TracingHttpClientPropagationThreadLocal.PROPERTY_NAME
            }
    }
}



micronaut:
  http:
    client:
      default-client:
        default:
          uri: http://localhost:8080 # Configuração padrão do HttpClient
        customized:
          uri: http://localhost:8080 # URL da API externa
          propagation: io.micronaut.tracing.propagation.TracePropagation.ThreadLocal




micronaut:
  http:
    client:
      default-client: customized
