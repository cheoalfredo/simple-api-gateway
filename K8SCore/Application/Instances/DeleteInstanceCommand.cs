using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace K8SCore.Application.Instances
{
    public class DeleteInstanceCommand : IRequest
    {
        public string InstanceIdentifier { get; set; }
        public Guid NubloqUserId { get; set; }

        public DeleteInstanceCommand(string instanceIdentifier, Guid nubloqUserId)
        {
            InstanceIdentifier = instanceIdentifier;
            NubloqUserId = nubloqUserId;
        }
    }
}
