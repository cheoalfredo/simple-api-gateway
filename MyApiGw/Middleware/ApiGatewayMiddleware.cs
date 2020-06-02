using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MyApiGw.Extensions;
using MyApiGw.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyApiGw.Middleware
{
    public class ApiGatewayMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;
        private readonly IEnumerable<GwEndpoint> _endpoints = new List<GwEndpoint>();
        public ApiGatewayMiddleware(RequestDelegate nextMiddleware, IConfiguration config)
        {
            _next = nextMiddleware;
            _config = config;
            _config.Bind("Routes", _endpoints);
        }


        public async Task InvokeAsync(HttpContext context, IHttpClientFactory upstream, IEnumerable<Endpoint> endpoints)
        {
            var (targetUri, basePath) = BuildTargetUri(context.Request);

            if (targetUri != null)
            {

                var rm = new HttpRequestMessage();
                rm.RequestUri = targetUri;
                rm.Headers.Host = targetUri.Host;
                rm.Method = new HttpMethod(context.Request.Method);
                var startTime = DateTime.Now;
                using (var responseMessage = await upstream.CreateClient().SendAsync(rm, HttpCompletionOption.ResponseHeadersRead, context.RequestAborted))
                {
                    context.Response.StatusCode = (int)responseMessage.StatusCode;
                    CopyFromTargetResponseHeaders(context, responseMessage, DateTime.Now.Subtract(startTime));
                    /*var responseOutput = await responseMessage.Content.ReadAsStringAsync();
                    byte[] byteArray = Encoding.UTF8.GetBytes(responseOutput.ReplaceUrlPaths(basePath));
                    MemoryStream stream = new MemoryStream(byteArray);
                    await stream.CopyToAsync(context.Response.Body);*/
                    await responseMessage.Content.CopyToAsync(context.Response.Body);
                }
                return;
            }
           
            await _next(context);
        }


        private (Uri, string) BuildTargetUri(HttpRequest request)
        {
            Uri targetUri = null;
            var endpoint = _endpoints.SingleOrDefault(e => request.Path.StartsWithSegments(e.BasePath, out var remaining));            
            if (endpoint != null)
            {
                var remainingSegment = request.Path.Value.Replace(endpoint.BasePath, "");
                targetUri = new Uri(endpoint.Upstream + remainingSegment);
            }            

            return (targetUri, (endpoint != null ? endpoint.BasePath : ""));
        }

        private void CopyFromTargetResponseHeaders(HttpContext context, HttpResponseMessage responseMessage, TimeSpan duration)
        {
            foreach (var header in responseMessage.Headers)
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            }

            context.Response.Headers.Add("UpstreamExecutionTime", duration.TotalMilliseconds.ToString());

            foreach (var header in responseMessage.Content.Headers)
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            }
            context.Response.Headers.Remove("transfer-encoding");
        }
    }


}
