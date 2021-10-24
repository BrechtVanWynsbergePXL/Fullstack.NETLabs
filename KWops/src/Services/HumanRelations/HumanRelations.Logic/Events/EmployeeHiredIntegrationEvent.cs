using Logic.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace HumanRelations.Logic.Events
{
    public class EmployeeHiredIntegrationEvent : IntegrationEvent
    {
        public string Number { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }
}
