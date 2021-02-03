using CSharpNewAPI.Middlewares;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpNewAPI.Extensions
{
    public static class AuthorizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomAuthorizationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthorizationMiddleware>();
        }
    }
}
