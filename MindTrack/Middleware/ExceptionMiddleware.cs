using MindTrack.Erros;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace MindTrack.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException validationEx)
        {
            _logger.LogError(validationEx, validationEx.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var response = _env.IsDevelopment() ?
                new ApiException(context.Response.StatusCode.ToString(), validationEx.Message, validationEx.StackTrace.ToString()) :
                new ApiException(context.Response.StatusCode.ToString(), validationEx.Message, "Erro de validação");

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);

        }
        catch (KeyNotFoundException notFoundEx)
        {
            _logger.LogWarning(notFoundEx, notFoundEx.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            var response = new ApiException(
                context.Response.StatusCode.ToString(),
                notFoundEx.Message,
                "Recurso não encontrado"
            );

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = _env.IsDevelopment() ?
                new ApiException(context.Response.StatusCode.ToString(), ex.Message, ex.StackTrace.ToString()) :
                new ApiException(context.Response.StatusCode.ToString(), ex.Message, "Erro Interno do servidor");

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }

    }
}