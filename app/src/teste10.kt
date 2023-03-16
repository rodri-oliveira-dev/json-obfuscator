#!/bin/bash

# Verifica se existem commits que não foram enviados
if git log origin/main..HEAD &> /dev/null; then
    echo "Existem commits que ainda não foram enviados para o repositório remoto."
    # Verifica se existem arquivos modificados, novos ou excluídos
    if git status --porcelain | grep -E '^(M|A|D)' &> /dev/null; then
        echo "Existem arquivos modificados, novos ou excluídos que precisam ser adicionados e commitados."
        # Executa o comando "gradlew build"
        ./gradlew build
    fi
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
