using Newtonsoft.Json;
using System.Net;
using SampleAPI.Infrastructure.Exceptions;

namespace SampleAPI.Infrastructure.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionMessageAsync(context, ex).ConfigureAwait(false);
            }
        }

        private static Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
        {
            string? content;
            if (exception is CustomException)
            {
                content = JsonConvert.SerializeObject(new
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ErrorMessage = "The internal error occurred.",
                    Details = exception.Message
                });
            }
            else
            {
                content = JsonConvert.SerializeObject(new
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ErrorMessage = "The unexpected exception occurred.",
                    Details = exception.Message
                });
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(content);
        }
    }
}
