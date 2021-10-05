using HumanRelations.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HumanRelations.Logic
{
    internal class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeFactory _employeeFactory;
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeFactory employeeFactory, IEmployeeRepository employeeRepository)
        {
            _employeeFactory = employeeFactory;
            _employeeRepository = employeeRepository;
        }

        public async Task<IEmployee> HireNewAsync(string lastName, string firstName, DateTime startDate)
        {
            int sequence = await _employeeRepository.GetNumberOfStartersOnAsync(startDate);
            IEmployee newEmployee = _employeeFactory.CreateNew(lastName, firstName, startDate, sequence + 1);
            await _employeeRepository.AddAsync(newEmployee);
            return newEmployee;
        }
    }
}
