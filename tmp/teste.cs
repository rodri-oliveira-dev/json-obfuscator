public static IApplicationBuilder IncludeSecurityHeaders(this IApplicationBuilder app)
{
    app.Use(async (context, next) =>
    {
        var headers = context.Response.Headers;

        headers.Append("X-Frame-Options", "DENY");
        headers.Append("X-Content-Type-Options", "nosniff");
        headers.Append("Content-Security-Policy", "default-src 'none'; " +
                                                   "script-src 'self' 'strict-dynamic'; " +
                                                   "font-src 'self' https://fonts.gstatic.com; " +
                                                   "img-src 'self' data:; " +
                                                   "connect-src 'self'; " +
                                                   "style-src 'self' https://fonts.googleapis.com");
        headers.Append("Permissions-Policy", "camera=(), microphone=(), geolocation=(), accelerometer=()");
        headers.Append("Referrer-Policy", "no-referrer");
        headers.Append("Cross-Origin-Resource-Policy", "same-origin");
        headers.Append("Origin-Agent-Cluster", "?1");
        headers.Append("Strict-Transport-Security", "max-age=31536000; includeSubDomains; preload");
        headers.Append("Expect-CT", "max-age=86400, enforce");
        headers.Append("X-Permitted-Cross-Domain-Policies", "none");

        if (headers.ContainsKey("Server"))
        {
            headers.Remove("Server"); // Oculta o servidor
        }

        await next();
    });

    return app;
}