using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;

namespace MyApiGw.Middleware
{
    public class HeaderProcessingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;
        public HeaderProcessingMiddleware(RequestDelegate nextMiddleware, IConfiguration config)
        {
            _next = nextMiddleware;
            _config = config;
        }

        public async Task InvokeAsync(HttpContext context, CustomHeaderLoader klz)
        {
            var headerNumber = 10;
            klz.Logo.ToList().ForEach(x => context.Response.Headers.Add($"klz{headerNumber++}", x));            
            await _next(context);
        }
    }
}
