using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;

namespace DotnetApi.Middlewares
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;

        // Usuario y contraseña "hardcodeados" para el curso
        private readonly string DemoUsername;
        private readonly string DemoPassword;

        public BasicAuthMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;

            // configuration.Authentication.Username y configuration.Authentication.Password
            DemoUsername = configuration["Authentication:Username"] ?? string.Empty;
            DemoPassword = configuration["Authentication:Password"] ?? string.Empty;
        }

        public async Task InvokeAsync(HttpContext context, [FromServices] ILogger<BasicAuthMiddleware> logger)
        {
            try
            {
                string path = context.Request.Path.Value ?? string.Empty;

                if (path.Contains("swagger") || path.Contains("scalar") || path.Contains("openapi"))
                {
                    await _next(context);
                    return;
                }

                // Leer encabezado Authorization
                string? rawHeader = context.Request.Headers.Authorization;

                if (string.IsNullOrEmpty(rawHeader))
                {
                    throw new Exception("Missing Authorization header");
                }

                var authHeader = AuthenticationHeaderValue.Parse(rawHeader);

                // 4. Verificamos que sea una autenticación de tipo "Basic" y tenga contenido
                if (!authHeader.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase) || authHeader.Parameter == null)
                {
                    throw new Exception("Invalid authentication scheme or missing parameter");
                }

                // Decodificar credenciales base64
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);

                var username = credentials[0];
                var password = credentials[1];

                // Validar usuario/contraseña
                if (username == DemoUsername && password == DemoPassword)
                {
                    // Usuario autenticado → continuar pipeline
                    await _next(context);
                    return;
                }
            }
            catch
            {
                // Si ocurre error significa que no vino el header correctamente
                logger.LogCritical("Error parsing Authorization header. Ensure it is in the format 'Basic base64'");
            }

            // Si llega aquí → no autorizado
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Missing or invalid credentials.");
        }
    }

    public static class BasicAuthExtensions
    {
        public static IApplicationBuilder UseBasicAuth(this IApplicationBuilder app)
        {
            return app.UseMiddleware<BasicAuthMiddleware>();
        }
    }
}