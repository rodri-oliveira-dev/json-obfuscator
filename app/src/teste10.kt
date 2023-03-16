#!/bin/bash

# Verifica se há commits locais não enviados na pasta app
if [[ $(git status app/ --porcelain) ]]; then
    echo "Existem commits locais não enviados na pasta app"
    ./gradlew build
    exit 0
fi

# Verifica se há arquivos modificados, adicionados ou excluídos na pasta app
if [[ $(git status app/ --porcelain) ]]; then
    echo "Existem arquivos modificados, adicionados ou excluídos na pasta app"
    ./gradlew build
    exit 0
fi

echo "Não há commits locais não enviados ou arquivos modificados, adicionados ou excluídos na pasta app"








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
