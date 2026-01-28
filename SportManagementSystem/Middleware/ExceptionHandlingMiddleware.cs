using System.Text.Json;
using SportManagementSystem.BuildingBlocks;
using SportManagementSystem.BuildingBlocks.Exceptions;
using SportManagementSystem.BuildingBlocks.Exceptions.Shared;

namespace SportManagementSystem.Middleware;

/// <summary>
/// Конвейер для обработки исключений.
/// </summary>
/// <remarks>
/// Этот конвейер перехватывает исключения, возникающие во время обработки HTTP-запросов,
/// логирует их и возвращает соответствующий ответ клиенту.
/// </remarks>
public class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) : IMiddleware
{
    /// <summary>
    /// Обрабатывает входящий HTTP-запрос и перехватывает исключения.
    /// </summary>
    /// <param name="context">Контекст HTTP-запроса.</param>
    /// <param name="next">Делегат для передачи управления следующему компоненту в конвейере.</param>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Unhandled exception occurred: {Message}", e.Message);
            await HandleExceptionAsync(context, e);
        }
    }

    /// <summary>
    /// Обрабатывает исключение и формирует ответ клиенту.
    /// </summary>
    /// <param name="httpContext">Контекст HTTP-запроса.</param>
    /// <param name="exception">Исключение, которое нужно обработать.</param>
    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var (statusCode, mbError) = GetMbError(exception);

        var response = MbResult<object>.Failure(mbError);

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            response.IsSuccess,
            response.Error!.Title,
            response.Error!.Status,
            response.Error!.Detail,
            response.Error!.Errors
        }));
    }
    
    private static (int StatusCode, MbError MbError) GetMbError(Exception exception)
    {
        return exception switch
        {
            ValidationAppException validationException => (
                StatusCodes.Status422UnprocessableEntity,
                new MbError(
                    title: "Validation Error",
                    status: StatusCodes.Status422UnprocessableEntity,
                    detail: validationException.Message,
                    errors: validationException.Errors
                )
            ),
            BadRequestException badRequest => (
                StatusCodes.Status400BadRequest,
                new MbError(
                    title: "Bad Request",
                    status: StatusCodes.Status400BadRequest,
                    detail: badRequest.Message
                )
            ),
            NotFoundException notFound => (
                StatusCodes.Status404NotFound,
                new MbError(
                    title: "Not Found",
                    status: StatusCodes.Status404NotFound,
                    detail: notFound.Message
                )
            ),
            _ => (
                StatusCodes.Status500InternalServerError,
                new MbError(
                    title: "Server Error",
                    status: StatusCodes.Status500InternalServerError,
                    detail: exception.Message
                )
            )
        };
    }
}