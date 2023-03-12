import io.micronaut.http.HttpRequest
import io.micronaut.http.HttpResponse
import io.micronaut.http.HttpStatus
import io.micronaut.http.filter.HttpServerFilter
import io.micronaut.http.filter.ServerFilterChain
import org.reactivestreams.Publisher
import org.slf4j.LoggerFactory
import reactor.core.publisher.Flux
import reactor.core.publisher.Mono
import java.time.Duration

class ExecutionTimeFilter : HttpServerFilter {

    private val logger = LoggerFactory.getLogger(ExecutionTimeFilter::class.java)

    override fun doFilter(request: HttpRequest<*>, chain: ServerFilterChain): Publisher<HttpResponse<*>> {
        val startTime = System.currentTimeMillis()
        return chain.proceed(request)
            .doOnComplete {
                val executionTime = System.currentTimeMillis() - startTime
                logger.info("Request ${request.uri} executed in ${Duration.ofMillis(executionTime)}")
            }
            .onErrorResume { e ->
                val status = when (e) {
                    is IllegalArgumentException -> HttpStatus.BAD_REQUEST
                    is NotFoundException -> HttpStatus.NOT_FOUND
                    else -> HttpStatus.INTERNAL_SERVER_ERROR
                }
                logger.error("Error processing request ${request.uri}", e)
                Mono.just(HttpResponse.status<Any?>(status))
            }
    }
}
