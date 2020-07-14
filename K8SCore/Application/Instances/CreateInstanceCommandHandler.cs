using AutoMapper;
using K8SCore.Domain;
using K8SCore.Domain.Ports;
using K8SCore.Domain.Services;
using MediatR;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace K8SCore.Application.Instances
{
    public class CreateInstanceCommandHandler : IRequestHandler<CreateInstanceCommand, InstanceDetailsDto>
    {
        private readonly ISandboxLauncher _launcher;
        private readonly SandboxService _sandboxService;
        private readonly IMapper _mapper;

        public CreateInstanceCommandHandler(ISandboxLauncher launcher, SandboxService service, IMapper mapper)
        {
            _launcher = launcher;
            _mapper = mapper;
            _sandboxService = service;
        }

        public async Task<InstanceDetailsDto> Handle(CreateInstanceCommand request, CancellationToken cancellationToken)
        {
            var instance = await _launcher.Create(request.InstanceLabels);
            instance.PortalUser = request.NubloqUserId;
            instance.IsActive = true;
            await _sandboxService.SaveInstance(instance);
            return _mapper.Map<InstanceDetailsDto>(instance);
        }
       
    }
}
