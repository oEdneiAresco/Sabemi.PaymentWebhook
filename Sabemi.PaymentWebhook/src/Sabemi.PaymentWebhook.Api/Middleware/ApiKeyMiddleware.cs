namespace Sabemi.PaymentWebhook.Api.Middleware;

public sealed class ApiKeyMiddleware
{
    private const string HeaderName = "X-Api-Key";

    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public ApiKeyMiddleware(
        RequestDelegate next,
        IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(
                HeaderName,
                out var apiKey))
        {
            context.Response.StatusCode =
                StatusCodes.Status401Unauthorized;

            await context.Response.WriteAsJsonAsync(new
            {
                message = "ApiKey não informada."
            });

            return;
        }

        var apiKeyConfigurada =
            _configuration["Webhook:ApiKey"];

        if (string.IsNullOrWhiteSpace(apiKeyConfigurada) ||
            apiKey != apiKeyConfigurada)
        {
            context.Response.StatusCode =
                StatusCodes.Status401Unauthorized;

            await context.Response.WriteAsJsonAsync(new
            {
                message = "ApiKey inválida."
            });

            return;
        }

        await _next(context);
    }
}