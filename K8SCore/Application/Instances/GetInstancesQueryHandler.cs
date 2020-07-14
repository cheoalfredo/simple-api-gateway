using AutoMapper;
using K8SCore.Domain.Services;
using MediatR;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace K8SCore.Application.Instances
{
    public class GetInstancesQueryHandler : IRequestHandler<GetInstancesQuery, IEnumerable<InstanceDetailsDto>>
    {

        private readonly SandboxService _sandbox;
        private readonly IMapper _Mapper;

        public GetInstancesQueryHandler(SandboxService sandbox, IMapper mapper)
        {
            _sandbox = sandbox;
            _Mapper = mapper;
        }
        

        async Task<IEnumerable<InstanceDetailsDto>> IRequestHandler<GetInstancesQuery, IEnumerable<InstanceDetailsDto>>.Handle(GetInstancesQuery request, CancellationToken cancellationToken)
        {
            var instances = await _sandbox.GetInstances(request.NubloqUserId).ConfigureAwait(false);
            return _Mapper.Map<IEnumerable<InstanceDetailsDto>>(instances);
        }
    }

}
