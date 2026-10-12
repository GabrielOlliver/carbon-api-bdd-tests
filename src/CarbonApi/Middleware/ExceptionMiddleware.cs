using System.Net;
using System.Text.Json;
using CarbonApi.Exceptions;
using CarbonApi.ViewModels.Response;

namespace CarbonApi.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse
        {
            Timestamp = DateTime.UtcNow,
            Caminho = context.Request.Path
        };

        switch (exception)
        {
            case NotFoundException:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Status = (int)HttpStatusCode.NotFound;
                response.Erro = "Recurso não encontrado";
                response.Mensagem = exception.Message;
                break;

            case BusinessException:
                context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                response.Status = (int)HttpStatusCode.UnprocessableEntity;
                response.Erro = "Erro de negócio";
                response.Mensagem = exception.Message;
                break;

            case ConflictException:
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                response.Status = (int)HttpStatusCode.Conflict;
                response.Erro = "Conflito de dados";
                response.Mensagem = exception.Message;
                break;

            case UnauthorizedAccessException:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                response.Status = (int)HttpStatusCode.Unauthorized;
                response.Erro = "Não autorizado";
                response.Mensagem = exception.Message;
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Status = (int)HttpStatusCode.InternalServerError;
                response.Erro = "Erro interno";
                response.Mensagem = "Ocorreu um erro inesperado. Tente novamente mais tarde.";
                break;
        }

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }
}