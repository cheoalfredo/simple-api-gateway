using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using K8SCore.Application.Instances;
using K8SCore.Domain;
using K8SCore.Domain.Ports;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X9;

namespace K8SCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SandboxController : ControllerBase
    {

        private readonly IMediator _mediator;

        public SandboxController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("instances")]
        public async Task<object> Get()
        {
            return await _mediator.Send(new GetInstancesQuery { NubloqUserId = Guid.Parse(User.GetOid()) });
        }

        [HttpPost]      
        public async Task<InstanceDetailsDto> Post([FromBody] Dictionary<string, string> labels)
        {
            return await _mediator.Send(new CreateInstanceCommand(labels, Guid.Parse(User.GetOid())));
        }

        [HttpDelete]
        public async Task Delete(string deploymentIdentifier)
        {            
            await _mediator.Send(new DeleteInstanceCommand(deploymentIdentifier, Guid.Parse(User.GetOid())));
        }
    }

    public static class ClaimsUtilities
    {
        public static string GetOid(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type.Equals("http://schemas.microsoft.com/identity/claims/objectidentifier")).Value;
        }
    }


}
