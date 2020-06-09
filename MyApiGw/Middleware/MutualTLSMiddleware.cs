using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MyApiGw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace MyApiGw.Middleware
{
    public class MutualTLSMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;
        private readonly IEnumerable<GwEndpoint> _endpoints = new List<GwEndpoint>();


        public MutualTLSMiddleware(RequestDelegate nextMiddleware, IConfiguration config)
        {
            _next = nextMiddleware;
            _config = config;
            _config.Bind("Routes", _endpoints);
        }


        public async Task InvokeAsync(HttpContext context)
        {
            
            if (_endpoints.Any())
            {
                var endpoint = _endpoints.SingleOrDefault(e => context.Request.Path.StartsWithSegments(e.BasePath, out var remaining));
                if (endpoint != null && endpoint.SSLCert != null)
                {
                    var certBytes = System.Convert.FromBase64String(endpoint.SSLCert);
                    var x509 = new X509Certificate2(certBytes);
                    var clientCert = context.Connection.ClientCertificate;
                }


            }


            await _next(context);
        }
    }
}
