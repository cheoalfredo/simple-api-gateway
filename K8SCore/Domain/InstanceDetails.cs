using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace K8SCore.Domain
{
    public class InstanceDetails : EntityBase<Guid>
    {
        public string DeploymentIdentifier { get; set; }
        public string Password { get; set; }
        public Guid PortalUser { get; set; }        
        public bool IsActive { get; set; }
    }
}
