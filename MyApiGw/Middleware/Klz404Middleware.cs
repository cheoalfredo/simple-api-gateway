using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyApiGw.Middleware
{
    public class Klz404Middleware
    {
        private readonly IConfiguration _config;
        private readonly RequestDelegate _next;
        public Klz404Middleware(RequestDelegate nextMiddleware, IConfiguration config)
        {
            _config = config;
            _next = nextMiddleware;
        }

        public async Task InvokeAsync(HttpContext context, CustomHeaderLoader klz)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;            
            using var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(string.Format(_config.GetValue<string>("404Body"), _config.GetValue<string>("404Logo").Replace(@"\", "")));
            writer.Flush();
            stream.Position = 0;
            await stream.CopyToAsync(context.Response.Body);
            return;           
        }
    }
}
