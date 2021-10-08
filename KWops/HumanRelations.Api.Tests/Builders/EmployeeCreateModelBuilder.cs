using System;
using HumanRelations.API.Models;
using Test;
namespace HumanRelations.Api.Tests.Builders
{
    internal class EmployeeCreateModelBuilder : BuilderBase<EmployeeCreateModel>
    {
        public EmployeeCreateModelBuilder()
        {
            Item = new EmployeeCreateModel
            {
                FirstName = Random.NextString(),
                LastName = Random.NextString(),
                StartDate = DateTime.Now
            };
        }
    }
}

