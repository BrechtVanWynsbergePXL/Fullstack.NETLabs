using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevOps.Api.Models
{
    public class TeamDetailModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<DeveloperDetailModel> Developers { get; set; }
        
        private class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Team, TeamAssembleInputModel>();
            }
        }
      
    }
}
