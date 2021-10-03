using HumanRelations.Domain;
using HumanRelations.Logic;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanRelations.API.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeesController(IEmployeeRepository repository)
        {
            _employeeRepository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("/employees/{number}")]
        public IActionResult GetEmployeeById(string number)
        {
            return (IActionResult)_employeeRepository.GetByNumberAsync(number);
        }

        [HttpPost]
        [Route("/employees")]
        public IActionResult PostEmployee(Employee employee)
        {
            return (IActionResult)_employeeRepository.AddAsync(employee);
        }


    }
}
