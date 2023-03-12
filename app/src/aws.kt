import com.amazonaws.Request
import com.amazonaws.Response
import com.amazonaws.handlers.RequestHandler2
import org.slf4j.LoggerFactory
import java.time.Duration

class ExecutionTimeHandler : RequestHandler2() {

    private val logger = LoggerFactory.getLogger(ExecutionTimeHandler::class.java)

    override fun beforeRequest(request: Request<*>?) {
        request?.apply {
            addHeader("X-Execution-Start-Time", System.currentTimeMillis().toString())
        }
    }

    override fun afterResponse(request: Request<*>?, response: Response<*>?) {
        request?.apply {
            response?.apply {
                val startTime = getHeader("X-Execution-Start-Time")?.toLongOrNull() ?: 0
                val executionTime = System.currentTimeMillis() - startTime
                logger.info("DynamoDB request executed in ${Duration.ofMillis(executionTime)}")
            }
        }
    }
}
