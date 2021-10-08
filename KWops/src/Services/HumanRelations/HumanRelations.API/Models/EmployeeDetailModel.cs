using AutoMapper;
using HumanRelations.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanRelations.API.Models
{
    public class EmployeeDetailModel
    {
        public string Number { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        private class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<IEmployee, EmployeeDetailModel>();
            }
        }

    }
}
