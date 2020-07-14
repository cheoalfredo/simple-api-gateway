using K8SCore.Domain.Ports;
using K8SCore.Domain.Services;
using MediatR;
using Namotion.Reflection;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace K8SCore.Application.Instances
{
    public class DeleteInstanceCommandHandler : AsyncRequestHandler<DeleteInstanceCommand>
    {

        private readonly ISandboxLauncher _launcher;
        private readonly SandboxService _service;
        public DeleteInstanceCommandHandler(ISandboxLauncher launcher, SandboxService service)
        {
            _launcher = launcher;
            _service = service;
        }
        protected override async Task Handle(DeleteInstanceCommand request, CancellationToken cancellationToken)
        {
            var instanceDetail = await _service.GetInstancesByUserAndId(request.InstanceIdentifier, request.NubloqUserId)
                .ConfigureAwait(false);
            if (instanceDetail.Any())
            {
                var currentInstance = instanceDetail.Single();
                await _launcher.Destroy(request.InstanceIdentifier).ConfigureAwait(false);
                currentInstance.IsActive = false;
                await _service.LogicalDeletionOfInstance(currentInstance);
            }
        }
    }
}
