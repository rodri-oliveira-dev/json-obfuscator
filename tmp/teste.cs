public static IApplicationBuilder IncludeSecurityHeaders(this IApplicationBuilder app)
{
    app.Use(async (context, next) =>
    {
        context.Response.Headers.Add("X-Frame-Options", "DENY");
        context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Add("Content-Security-Policy", "default-src 'none'; " +
                                                                "script-src 'self' 'strict-dynamic'; " +
                                                                "font-src 'self' https://fonts.gstatic.com; " +
                                                                "img-src 'self' data:; " +
                                                                "connect-src 'self'; " +
                                                                "style-src 'self' https://fonts.googleapis.com");
        context.Response.Headers.Add("Permissions-Policy", "camera=(), microphone=(), geolocation=(), accelerometer=()");
        context.Response.Headers.Add("Referrer-Policy", "no-referrer");
        context.Response.Headers.Add("Cross-Origin-Resource-Policy", "same-origin");
        context.Response.Headers.Add("Origin-Agent-Cluster", "?1");
        context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains; preload");
        context.Response.Headers.Add("Expect-CT", "max-age=86400, enforce");
        context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
        context.Response.Headers.Remove("Server"); // Oculta o servidor

        await next();
    });

    return app;
}