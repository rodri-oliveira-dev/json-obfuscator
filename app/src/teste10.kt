class RequestInfoContext {
    companion object {
        private val threadLocal = ThreadLocal<RequestInfo>()

        fun set(requestInfo: RequestInfo) {
            threadLocal.set(requestInfo)
        }

        fun get(): RequestInfo {
            return threadLocal.get()
        }

        fun clear() {
            threadLocal.remove()
        }
    }
}
