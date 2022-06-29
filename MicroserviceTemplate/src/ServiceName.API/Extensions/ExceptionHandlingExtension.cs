using System.Net;

namespace ServiceName.API.Extensions
{
    public static class ExceptionHandlingExtension
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/text";
                    await context.Response.WriteAsync("An error has occurred while processing your request. Check the logs for more information.");
                });
            });
        }
    }
}
