using HumanRelations.API.Models;
using HumanRelations.Domain;
using HumanRelations.Logic;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanRelations.API.Controllers
{
    public class EmployeesController : Controller, IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeRepository repository, IEmployeeService employeeService)
        {
            _employeeRepository = repository;
            _employeeService = employeeService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("/employees/{number}")]
        public IActionResult GetEmployeeById(string number)
        {
            //IEmployee employee = (IEmployee)_employeeRepository.GetByNumberAsync(number);
            //return (IActionResult)employee;
            return (IActionResult)_employeeRepository.GetByNumberAsync(number);
        }

        [HttpPost]
        [Route("/employees")]
        public IActionResult PostEmployee(IEmployee employee)
        {
            return (IActionResult)_employeeRepository.AddAsync(employee);
        }
            
        public Task<IEmployee> HireNewAsync(string lastName, string firstName, DateTime startDate)
        {
            return _employeeService.HireNewAsync(lastName, firstName, startDate);
        }

        [HttpGet("{number}")]
        public async Task<IActionResult> GetByNumber(string number)
        {
            IEmployee employee = await _employeeRepository.GetByNumberAsync(number);
            return employee == null ? NotFound() : Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Add(EmployeeCreateModel model)
        {
            IEmployee hiredEmployee = await _employeeService.HireNewAsync(model.LastName, model.FirstName, model.StartTime);
            return CreatedAtAction(nameof(GetByNumber), new { number = hiredEmployee.Number }, hiredEmployee);
        }
    }
}
