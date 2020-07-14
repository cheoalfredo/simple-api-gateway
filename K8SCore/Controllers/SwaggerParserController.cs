using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using K8SCore.Domain.Ports;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NSwag;
using NSwag.CodeGeneration.CSharp;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace K8SCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SwaggerParserController : ControllerBase
    {
        private readonly ISwaggerManager _swagg;

        public SwaggerParserController(ISwaggerManager swagg)
        {
            _swagg = swagg;
        }

        

        [HttpGet]
        public async Task<List<KeyValuePair<string, string>>> GetApis()
        {
            return await _swagg.RetrieveApis();
        }

        [HttpGet("api/{api}")]
        public async Task<List<KeyValuePair<string, string>>> GetResources(string api)
        {
            return await _swagg.GetResources(api);
        }

        // GET api/<SwaggerParser>/5
        [HttpGet("api/{api}/{resource}")]
        public async Task<object> GetItem(string api, string resource)
        {
            return await _swagg.GetResourceEndpoints(api, resource);
        }


    }
}
