using HumanRelations.Domain;
using Microsoft.AspNetCore.Mvc;
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

        public async Task DismissAsync(EmployeeNumber employeeNumber, bool withNotice)
        {
            IEmployee employeeToDismiss = await _employeeRepository.GetByNumberAsync(employeeNumber);
            employeeToDismiss.Dismiss(withNotice);
            await _employeeRepository.CommitTrackedChangesAsync();

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
