using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ShoppingCart.Web.Middleware
{
    public class ExceptionMiddleware
    {
        private RequestDelegate Next { get; }
        private ILoggerFactory Logger { get; }
        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory logger)
        {
            Logger = logger;
            Next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await Next(httpContext);
            }
            catch (Exception ex)
            {
                Logger.CreateLogger("Global Error").LogError($"Something went wrong: {ex}");              
                await HandleExceptionAsync(httpContext);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(new 
            {
                StatusCode = context.Response.StatusCode,
                Message = "Something went wrong. Our support team is looking into it"
            }.ToString());
        }
    }
}
