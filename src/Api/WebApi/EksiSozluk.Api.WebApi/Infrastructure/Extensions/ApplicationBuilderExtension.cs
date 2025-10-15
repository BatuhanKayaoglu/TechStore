using EksiSozluk.Common.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace EksiSozluk.Api.WebApi.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder ConfigureExceptionHandling(this IApplicationBuilder app, bool includeExceptionDetails = false, bool useDefaultHandlingResponse = true, Func<HttpContext, Exception, Task> handleException = null)
        {
            app.UseExceptionHandler(options =>
            {
                options.Run(context =>
                {
                    var exceptionObject = context.Features.Get<ExceptionHandlerFeature>();

                    if (!useDefaultHandlingResponse && handleException == null)
                        throw new ArgumentNullException(nameof(handleException), $"{nameof(handleException)} cannot be null ");

                    if (!useDefaultHandlingResponse && handleException != null)
                        return handleException(context, exceptionObject.Error);

                    return DefaultHandleException(context, exceptionObject.Error, includeExceptionDetails);
                });

            });




            return app;
        }

        private static async Task DefaultHandleException(HttpContext context, Exception exception, bool includeExceptionDetails = false)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string message = "Internal server error occured!";
            if (exception is UnauthorizedAccessException)
                statusCode = HttpStatusCode.Unauthorized;


            var res = new
            {
                HttpStatusCode = (int)statusCode,
                Detail = includeExceptionDetails ? exception.ToString() : message
            };

            await WriteResponse(context, statusCode, res);
        }

        private static async Task WriteResponse(HttpContext context, HttpStatusCode statusCode, object responseObj)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsJsonAsync(responseObj);
        }



    }


}
