using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MyApiGw.Models;
using System;
using System.IO;
using System.Linq;
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

            if (_blItems.SourceIps.Contains(context.Connection.RemoteIpAddress.ToString()) && 
                _blItems.AllowedOrDenied.Equals(ListType.ByDefaultAllAllowed))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                byte[] byteArray = Encoding.UTF8.GetBytes("Your request has been denied, maybe on black list ?");
                MemoryStream stream = new MemoryStream(byteArray);
                await stream.CopyToAsync(context.Response.Body);             
                return;
            }

            await _next(context);
        }
    }
}
