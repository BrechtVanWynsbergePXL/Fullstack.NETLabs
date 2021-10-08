using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanRelations.API.Models
{
    public class EmployeeCreateModel
    {
        public string FirstName { get; set; }
        public string  LastName { get; set; }
        public DateTime StartDate { get; set; }
    }
}
