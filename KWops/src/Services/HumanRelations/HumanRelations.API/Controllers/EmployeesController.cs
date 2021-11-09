using AutoMapper;
using HumanRelations.API.Models;
using HumanRelations.Domain;
using HumanRelations.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanRelations.API.Controllers
{
    [Controller]
    public class EmployeesController : Controller, IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeesController(IEmployeeRepository repository, IEmployeeService employeeService, IMapper mapper)
        {
            _employeeRepository = repository;
            _employeeService = employeeService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("/employees/{number}")]
        [Authorize(policy: "read")]
        public IActionResult GetEmployeeById(string number)
        {
            //IEmployee employee = (IEmployee)_employeeRepository.GetByNumberAsync(number);
            //return (IActionResult)employee;
            return (IActionResult)_employeeRepository.GetByNumberAsync(number);
        }

        [HttpPost]
        [Route("/employees")]
        [Authorize(policy: "read")]
        public IActionResult PostEmployee(IEmployee employee)
        {
            return (IActionResult)_employeeRepository.AddAsync(employee);
        }

        public Task<IEmployee> HireNewAsync(string lastName, string firstName, DateTime startDate)
        {
            return _employeeService.HireNewAsync(lastName, firstName, startDate);
        }

        [HttpGet("{number}")]
        [Authorize(policy: "read")]
        public async Task<IActionResult> GetByNumber(string number)
        {
            IEmployee employee = await _employeeRepository.GetByNumberAsync(number);
            return employee == null ? NotFound() : Ok(_mapper.Map<EmployeeDetailModel>(employee));
        }

        [HttpPost]
        [Authorize(policy: "write")]
        public async Task<IActionResult> Add(EmployeeCreateModel model)
        {
            IEmployee hiredEmployee = await _employeeService.HireNewAsync(model.LastName, model.FirstName, model.StartDate);
            var outputModel = _mapper.Map<EmployeeDetailModel>(hiredEmployee);
            return CreatedAtAction(nameof(GetByNumber), new { number = outputModel.Number }, outputModel);
        }

        [HttpPost("{number}/dismiss")]
        [Authorize(policy:"write")]
        public async Task DismissAsync(EmployeeNumber employeeNumber, [FromQuery] bool withNotice = true)
        {
            await _employeeService.DismissAsync(employeeNumber, withNotice);
        }
    }
}
