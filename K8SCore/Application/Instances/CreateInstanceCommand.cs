using K8SCore.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading.Tasks;

namespace K8SCore.Application.Instances
{
    public class CreateInstanceCommand : IRequest<InstanceDetailsDto>
    {
        public Dictionary<string, string> InstanceLabels { get; set; }
        public Guid NubloqUserId { get; set; }

        public CreateInstanceCommand(Dictionary<string, string> labels, Guid userId)
        {
            InstanceLabels = labels;
            NubloqUserId = userId;
        }
    }

    
}
