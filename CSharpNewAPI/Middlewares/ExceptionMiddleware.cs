using System;
using System.Threading.Tasks;
using CSharpNewAPI.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CSharpNewAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private RequestDelegate _next;

        // folosit pt a arata in terminal ce exceptie a avut loc
        private ILogger<ApiException> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ApiException> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // try to pass to the next middleware
                await _next(context);
            }
            catch (Exception e)
            {
                // used in order to show the exception in the terminal
                _logger.LogError(e, e.Message);
                var exception = new ApiException(500, e.Message, e.StackTrace);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync<ApiException>(exception);
            }
        }
    }
}