using K8SCore.Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace K8SCore.Domain.Services
{
    [DomainService]
    public class SandboxService
    {

        private readonly IGenericRepository<InstanceDetails> _instances;

        public SandboxService(IGenericRepository<InstanceDetails> instances)
        {
            _instances = instances;
        }

        public async Task<IEnumerable<InstanceDetails>> GetInstances(Guid nubloqUserId)
        {
            return await _instances.GetAsync(filter: f => f.PortalUser.Equals(nubloqUserId) && f.IsActive).ConfigureAwait(false);
        }

        public async Task<IEnumerable<InstanceDetails>> GetInstancesByUserAndId(string instanceId, Guid nubloqUserId)
        {
            return await _instances.GetAsync(filter: f => f.PortalUser.Equals(nubloqUserId)
            && f.DeploymentIdentifier.Equals(instanceId) && f.IsActive).ConfigureAwait(false);
        }

        public async Task LogicalDeletionOfInstance(InstanceDetails instance)
        {
            await _instances.UpdateAsync(instance).ConfigureAwait(false);
        }

        public async Task SaveInstance(InstanceDetails details)
        {
            await _instances.AddAsync(details).ConfigureAwait(false);
        }
    }
}
