using CSharpNewAPI.Interfaces;
using CSharpNewAPI.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpNewAPI.Middlewares
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context, ITokenService tokenService)
        {
            IHeaderDictionary headers = context.Request.Headers;
            string[] values = context.Request.Headers["Authorization"].ToString().Split(" ");
            string test = context.Request.Headers["Authorization"].ToString();
            if (values == null || values.Length != 2)
            {
                var response = context.Response;
                response.StatusCode = 401;
                await context.Response.WriteAsync("JSON NOT GOOD");
                return;
            }
            else
            {
                string token = values[1];
                bool isTokenValid = tokenService.validateToken(token);
                if (!isTokenValid)
                {
                    var response = context.Response;
                    response.StatusCode = 401;
                    await context.Response.WriteAsync("JSON NOT GOOD");
                    return;
                }
            }
            await _next(context);
        }
    }
}
