using HumanRelations.Domain;
using HumanRelations.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task AddAsync(IEmployee newEmployee)
        {
            await _humanRelationsContext.AddAsync(newEmployee);
        }

        public async Task CommitTrackedChangesAsync()
        {
            await _humanRelationsContext.SaveChangesAsync();
        }

        public async Task<IEmployee> GetByNumberAsync(EmployeeNumber number)
        {
            return await _humanRelationsContext.Employee.FindAsync(number);
        }

        public async Task<int> GetNumberOfStartersOnAsync(DateTime startTime)
        {
            return await Task.FromResult(_humanRelationsContext.Employee.Count(e => e.StartDate == startTime));
        }
    }
}
