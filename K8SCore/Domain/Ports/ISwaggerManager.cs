using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace K8SCore.Domain.Ports
{
    public interface ISwaggerManager
    {
        Task<List<KeyValuePair<string, string>>> RetrieveApis();
        Task<List<KeyValuePair<string, string>>> GetResourceEndpoints(string apiName, string resource);
        Task<List<KeyValuePair<string, string>>> GetResources(string apiName);
    }
}
