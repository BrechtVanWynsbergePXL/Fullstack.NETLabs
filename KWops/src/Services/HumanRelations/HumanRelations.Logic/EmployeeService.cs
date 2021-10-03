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

        public Task<IEmployee> HireNewAsync(string lastName, string firstName, DateTime startDate)
        {
            int sequence = _employeeRepository.GetNumberOfStartersOnAsync(startDate);
            _employeeFactory.CreateNew(lastName, firstName, startDate, );
        }
    }
}
