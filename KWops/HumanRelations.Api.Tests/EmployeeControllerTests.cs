using AutoMapper;
using HumanRelations.Api.Tests.Builders;
using HumanRelations.API.Controllers;
using HumanRelations.API.Models;
using HumanRelations.Domain;
using HumanRelations.Logic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test;

namespace HumanRelations.Api.Tests
{
    public class EmployeesControllerTests : TestBase
    {
        private Mock<IEmployeeService> _employeeServiceMock;
        private Mock<IEmployeeRepository> _employeeRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private EmployeesController _controller;
        [SetUp]
        public void Setup()
        {
            _employeeServiceMock = new Mock<IEmployeeService>();
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _mapperMock = new Mock<IMapper>();
            _controller = new EmployeesController(_employeeRepositoryMock.Object, _employeeServiceMock.Object, _mapperMock.Object);
        }
        [Test]
        public void Add_ShouldUseServiceAndMapResult()
        {
            //Arrange
            EmployeeCreateModel createModel = new EmployeeCreateModelBuilder().Build();
            IEmployee hiredEmployee = new Mock<IEmployee>().Object;
            _employeeServiceMock.Setup(service => service.HireNewAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).ReturnsAsync(hiredEmployee);
            EmployeeDetailModel outputModel = new EmployeeDetailModelBuilder().Build();
            _mapperMock.Setup(mapper => mapper.Map<EmployeeDetailModel>(It.IsAny<IEmployee>())).Returns(outputModel);
            //Act
            var result = _controller.Add(createModel).Result as CreatedAtActionResult;
            //Assert
            Assert.That(result, Is.Not.Null);
            _employeeServiceMock.Verify(service => service.HireNewAsync(createModel.LastName, createModel.FirstName, createModel.StartDate), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<EmployeeDetailModel>(hiredEmployee), Times.Once);
            Assert.That(result.ActionName, Is.EqualTo(nameof(EmployeesController.GetByNumber)));
            Assert.That(result.RouteValues["number"], Is.EqualTo(outputModel.Number));
            Assert.That(result.Value, Is.SameAs(outputModel));
        }

        [Test]
        public void GetByNumber_ShouldUseRepositoryAndMapResult()
        {
            //Arrange
            EmployeeNumber employeeNumber = new EmployeeNumber(DateTime.Now,1);
            IEmployee employee = new Mock<IEmployee>().Object;
            _employeeRepositoryMock.Setup(repo => repo.GetByNumberAsync(It.IsAny<EmployeeNumber>())).ReturnsAsync(employee);
            EmployeeDetailModel outputModel = new EmployeeDetailModelBuilder().Build();
            _mapperMock.Setup(mapper => mapper.Map<EmployeeDetailModel>(It.IsAny<IEmployee>())).Returns(outputModel);
            //Act
            var result = _controller.GetByNumber(employeeNumber).Result as OkObjectResult;
            //Assert
            Assert.That(result, Is.Not.Null);
            _employeeRepositoryMock.Verify(repo => repo.GetByNumberAsync(employeeNumber), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<EmployeeDetailModel>(employee), Times.Once);
            Assert.That(result.Value, Is.SameAs(outputModel));
        }

        [Test]
        public void GetByNumber_EmployeeDoesNotExist_ShouldReturnNotFound()
        {
            //Arrange
            EmployeeNumber employeeNumber = new EmployeeNumber(DateTime.Now,1);
            _employeeRepositoryMock.Setup(repo => repo.GetByNumberAsync(It.IsAny<EmployeeNumber>())).ReturnsAsync(() => null);
            //Act
            var result = _controller.GetByNumber(employeeNumber).Result as NotFoundResult;
            //Assert
            Assert.That(result, Is.Not.Null);
            _employeeRepositoryMock.Verify(repo => repo.GetByNumberAsync(employeeNumber), Times.Once);
        }
    }
}
