using HumanRelations.Domain;
using System;
using System.Threading.Tasks;

namespace HumanRelations.Logic
{
    public interface IEmployeeRepository
    {
        Task AddAsync(IEmployee newEmployee);
        Task<IEmployee> GetByNumberAsync(EmployeeNumber number);
        Task<int> GetNumberOfStartersOnAsync(DateTime startTime);
    }
}
