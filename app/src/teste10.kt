class RequestState {
    companion object {
        private val threadLocalState = ThreadLocal<MutableMap<String, Any>>()

        fun put(key: String, value: Any) {
            var state = threadLocalState.get()
            if (state == null) {
                state = mutableMapOf()
                threadLocalState.set(state)
            }
            synchronized(state) {
                state[key] = value
            }
        }

        fun get(key: String): Any? {
            val state = threadLocalState.get()
            return if (state != null) {
                synchronized(state) {
                    state[key]
                }
            } else {
                null
            }
        }

        fun clear() {
            threadLocalState.remove()
        }
    }
}
