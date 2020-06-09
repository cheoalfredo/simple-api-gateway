using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.Extensions.Configuration;
using MyApiGw.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApiGw.Middleware
{
    public class BWListMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;
        private readonly BWItems _blItems = new BWItems();
      
     
        public BWListMiddleware(RequestDelegate nextMiddleware, IConfiguration config)
        {
            _next = nextMiddleware;
            _config = config;
            _config.Bind("BWList", _blItems);
        }


        public async Task InvokeAsync(HttpContext context)
        {
           
            if (_blItems.SourceIps != null)
            {
                var remoteIp = context.Connection.RemoteIpAddress.ToString();
                if ((_blItems.SourceIps.Contains(remoteIp) && _blItems.AllowedOrDenied.Equals(BLBehavior.ByDefaultAllAllowed)) ||                  
                 (!_blItems.SourceIps.Contains(remoteIp) && _blItems.AllowedOrDenied.Equals(BLBehavior.ByDefaultAllDenied)))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    byte[] byteArray = Encoding.UTF8.GetBytes("Your request has been denied, maybe on black list ?");
                    MemoryStream stream = new MemoryStream(byteArray);
                    await stream.CopyToAsync(context.Response.Body);
                    return;
                }
              
            }

           
            await _next(context);
        }
    }
}
