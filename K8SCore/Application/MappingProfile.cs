using AutoMapper;
using K8SCore.Application.Instances;
using K8SCore.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace K8SCore.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<InstanceDetails, InstanceDetailsDto>();
            CreateMap<InstanceDetailsDto, InstanceDetails>();
        }
    }
}
