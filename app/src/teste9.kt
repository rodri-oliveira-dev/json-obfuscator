class GlobalState private constructor() {
    private val state: MutableMap<String, Any> = mutableMapOf()

    fun put(key: String, value: Any) {
        synchronized(state) {
            state[key] = value
        }
    }

    fun get(key: String): Any? {
        return synchronized(state) {
            state[key]
        }
    }

    companion object {
        @Volatile
        private var instance: GlobalState? = null

        fun getInstance(): GlobalState {
            return instance ?: synchronized(this) {
                instance ?: GlobalState().also { instance = it }
            }
        }
    }
}


import io.micronaut.http.HttpRequest
import io.micronaut.http.HttpResponse
import io.micronaut.http.MutableHttpResponse
import io.micronaut.http.client.exceptions.HttpClientResponseException
import io.micronaut.http.client.filter.HttpClientFilter
import io.micronaut.http.filter.ClientFilterChain
import io.reactivex.Flowable
import org.reactivestreams.Publisher
import javax.inject.Singleton

@Singleton
class CustomHttpClientFilter : HttpClientFilter {

    override fun doFilter(request: HttpRequest<*>?, chain: ClientFilterChain?): Publisher<out HttpResponse<*>> {
        if (request == null || chain == null) {
            return Flowable.error(HttpClientResponseException("Request or chain is null", HttpResponse.serverError<Any>()))
        }

        println("Request: ${request.method} ${request.uri}")

        return chain.proceed(request)
            .doOnNext { response ->
                println("Response: ${response.status}")
            }
    }
}


micronaut:
  http:
    client:
      filters:
        - com.example.CustomHttpClientFilter



#!/bin/bash

# Verifica se há commits não pushados na pasta app
if [[ -n $(git log origin/master..HEAD --oneline -- app) ]]; then
    # Se houver, executa o comando gradlew clean build
    cd app
    ./gradlew clean build
    cd ..
fi
