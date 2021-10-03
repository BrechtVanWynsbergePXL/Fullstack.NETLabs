using HumanRelations.Domain;
using System.Threading.Tasks;

namespace HumanRelations.Logic
{
    public interface IEmployeeRepository
    {
        Task AddAsync(Employee newEmployee);
        Task<Employee> GetByNumberAsync(string number);
    }
}
