using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace K8SCore.Domain
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DomainServiceAttribute : Attribute
    {
    }
}
