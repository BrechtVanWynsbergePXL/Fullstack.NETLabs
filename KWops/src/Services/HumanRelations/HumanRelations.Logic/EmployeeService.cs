using HumanRelations.Domain;
using HumanRelations.Logic.Events;
using Logic.Events;
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
        private readonly IEventBus _eventBus;

        public EmployeeService(IEmployeeFactory employeeFactory, IEmployeeRepository employeeRepository, IEventBus eventBus)
        {
            _employeeFactory = employeeFactory;
            _employeeRepository = employeeRepository;
            _eventBus = eventBus;
        }

        public async Task DismissAsync(EmployeeNumber employeeNumber, bool withNotice)
        {
            IEmployee employeeToDismiss = await _employeeRepository.GetByNumberAsync(employeeNumber);
            employeeToDismiss.Dismiss(withNotice);
            await _employeeRepository.CommitTrackedChangesAsync();

        }

        public async Task<IEmployee> HireNewAsync(string lastName, string firstName, DateTime startDate)
        {
            int sequence = await _employeeRepository.GetNumberOfStartersOnAsync(startDate) + 1;
            IEmployee newEmployee = _employeeFactory.CreateNew(lastName, firstName, startDate, sequence);
            await _employeeRepository.AddAsync(newEmployee);

            var @event = new EmployeeHiredIntegrationEvent { Number = newEmployee.Number, LastName = newEmployee.LastName, FirstName = newEmployee.FirstName };
            _eventBus.Publish(@event);

            return newEmployee;
        }
    }
}
