using HumanRelations.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HumanRelations.Logic
{
    public interface IEmployeeService
    {
        Task<IEmployee> HireNewAsync(string lastName, string firstName, DateTime startDate);
    }
}
