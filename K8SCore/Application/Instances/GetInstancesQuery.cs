using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace K8SCore.Application.Instances
{
    public class GetInstancesQuery : IRequest<IEnumerable<InstanceDetailsDto>>
    {
        [Required]
        public Guid NubloqUserId { get; set; }
    }
}
