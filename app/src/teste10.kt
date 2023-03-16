#!/bin/bash

# Navega até a pasta do repositório Git
cd /caminho/para/seu/repositorio

# Verifica se há alterações na pasta app
git status app | grep -qE 'new file|modified|deleted'

# Se houver alterações na pasta app, execute o gradlew clean build
if [ $? -eq 0 ]; then
  echo "Alterações encontradas na pasta app. Executando 'gradlew clean build'."
  cd app
  ./gradlew clean build
else
  echo "Nenhuma alteração encontrada na pasta app."
fi






import io.micronaut.http.HttpRequest
import io.micronaut.http.HttpResponse
import io.micronaut.http.annotation.FilterMatcher
import io.micronaut.http.filter.HttpFilter
import io.micronaut.http.filter.ServerFilterChain
import org.reactivestreams.Publisher

class RequestDataFilter : HttpFilter {
    override fun doFilter(request: HttpRequest<*>, chain: ServerFilterChain): Publisher<out HttpResponse<*>> {
        val requestId = request.headers.get("X-Request-ID") ?: generateRequestId()
        val requestOf = request.headers.get("X-Request-Of") ?: "Unknown"

        RequestContextHolder.setRequestData(RequestData(requestId, requestOf))

        return chain.proceed(request).doFinally {
            RequestContextHolder.clear()
        }
    }

    private fun generateRequestId(): String {
        // Implemente sua lógica de geração de ID de requisição aqui
        return UUID.randomUUID().toString()
    }
}




object RequestContextHolder {
    private val contextHolder = ThreadLocal<RequestData>()

    fun setRequestData(requestData: RequestData) {
        contextHolder.set(requestData)
    }

    fun getRequestData(): RequestData? {
        return contextHolder.get()
    }

    fun clear() {
        contextHolder.remove()
    }
}
