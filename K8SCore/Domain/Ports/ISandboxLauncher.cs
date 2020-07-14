using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace K8SCore.Domain.Ports
{
    public interface ISandboxLauncher
    {
        Task<InstanceDetails> Create(Dictionary<string, string> labels);

        Task Destroy(string deploymentIdentifier);
       
        
    }
}
