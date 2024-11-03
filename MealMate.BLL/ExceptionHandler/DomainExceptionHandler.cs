using MealMate.DAL.Utils.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;


namespace MealMate.BLL.ExceptionHandler
{
    internal class DomainExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            // Handle domain-specific exceptions like EntityNotFoundException, EntityValidationException
            if (exception is DomainException domainException)
            {
                httpContext.Response.StatusCode = (int)domainException.StatusCode;
                await httpContext.Response.WriteAsJsonAsync(new { message = domainException.Message }, cancellationToken);
                return true;
            }

            // Handle BadRequest for validation or invalid argument errors
            if (exception is ArgumentException || exception is InvalidOperationException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new { message = "Bad Request", details = exception.Message }, cancellationToken);
                return true;
            }

            // Handle UnauthorizedAccessException for invalid credentials
            if (exception is UnauthorizedAccessException)
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsJsonAsync(new { message = "Unauthorized", details = exception.Message }, cancellationToken);
                return true;
            }

            // Handle any other unhandled exception as Internal Server Error
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(new { message = "An unexpected error occurred", details = exception.Message }, cancellationToken);
            return true;
        }
    }


}


