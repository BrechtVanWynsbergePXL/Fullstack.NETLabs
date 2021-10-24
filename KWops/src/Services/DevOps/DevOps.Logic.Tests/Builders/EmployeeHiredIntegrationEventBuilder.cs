using DevOps.Logic.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test;

namespace DevOps.Logic.Tests.Builders
{
    internal class EmployeeHiredIntegrationEventBuilder : BuilderBase<EmployeeHiredIntegrationEvent>
    {
        public EmployeeHiredIntegrationEventBuilder()
        {
            Item = new EmployeeHiredIntegrationEvent
            {
                Number = Random.NextString(),
                FirstName = Random.NextString(),
                LastName = Random.NextString()
            };
        }
    }
}
