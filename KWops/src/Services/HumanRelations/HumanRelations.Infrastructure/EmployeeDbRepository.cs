using HumanRelations.Domain;
using HumanRelations.Logic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HumanRelations.Infrastructure
{
    internal class EmployeeDbRepository : IEmployeeRepository
    {
        private readonly HumanRelationsContext _humanRelationsContext;
        public EmployeeDbRepository(HumanRelationsContext context)
        {
            _humanRelationsContext = context;
        }

        public async Task AddAsync(Employee newEmployee)
        {
            await _humanRelationsContext.AddAsync(newEmployee);
        }

        public async Task<Employee> GetByNumberAsync(string number)
        {
            return await _humanRelationsContext.Employee.FindAsync(number);
        }
    }
}
