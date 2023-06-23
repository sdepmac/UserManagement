namespace UsersApi.ErrorHandling;

using System.Net;
using System.Text.Json;
using UsersApi.ErrorHandling.Exceptions;

/// <summary>
/// Error handler middleware.
/// </summary>
public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorHandlerMiddleware"/> class.
    /// </summary>
    /// <param name="next">request delegate.</param>
    /// <param name="logger">logger.</param>
    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    /// <summary>
    /// Invoke error handler.
    /// </summary>
    /// <param name="context">context.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await this.next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            switch (error)
            {
                case DuplicateUserException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case KeyNotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    this.logger.LogError(error, error.Message);
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonSerializer.Serialize(new { message = error?.Message });
            await response.WriteAsync(result);
        }
    }
}
