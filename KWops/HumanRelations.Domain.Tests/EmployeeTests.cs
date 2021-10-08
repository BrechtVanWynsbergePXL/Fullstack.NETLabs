using System;
using Domain;
using HumanRelations.Domain.Tests.Builders;
using NUnit.Framework;
using Test;


namespace HumanRelations.Domain.Tests
{
    public class EmployeeTests : TestBase
    {
        [Test]
        public void Constructor_FromStartDateAndSequence_ShouldInitializeProperly()
        {
            //Arrange
            DateTime startDate = Random.NextDateTimeInFuture();
            int sequence = Random.NextPositive();

            //Act
            var number = new EmployeeNumber(startDate, sequence);

            //Assert
            Assert.That(number.Year, Is.EqualTo(startDate.Year));
            Assert.That(number.Month, Is.EqualTo(startDate.Month));
            Assert.That(number.Day, Is.EqualTo(startDate.Day));
            Assert.That(number.Sequence, Is.EqualTo(sequence));
        }

        [Test]
        public void Constructor_FromStartDateAndSequence_InvalidSequence_ShouldThrowContractException()
        {
            //Arrange
            DateTime startDate = Random.NextDateTimeInFuture();
            int invalidSequence = Random.NextZeroOrNegative();

            //Act + Assert
            Assert.That(() => new EmployeeNumber(startDate, invalidSequence), Throws.InstanceOf<ContractException>());
        }

        [TestCase("19990101001", 1999, 1, 1, 1)]
        [TestCase("20001231123", 2000, 12, 31, 123)]
        public void Constructor_FromString_ShouldInitializeProperly(string input, int expectedYear, int expectedMonth,
            int expectedDay, int expectedSequence)
        {
            //Act
            var number = new EmployeeNumber(input);

            //Assert
            Assert.That(number.Year, Is.EqualTo(expectedYear));
            Assert.That(number.Month, Is.EqualTo(expectedMonth));
            Assert.That(number.Day, Is.EqualTo(expectedDay));
            Assert.That(number.Sequence, Is.EqualTo(expectedSequence));
        }

        [TestCase("")]
        [TestCase("1")]
        [TestCase("abcdefghikj")]
        [TestCase("00001231001")]
        [TestCase("19991331001")]
        [TestCase("19990031001")]
        [TestCase("19991200001")]
        [TestCase("19991232001")]
        [TestCase("19991231000")]
        public void Constructor_FromString_InvalidInput_ShouldThrowContractException(string input)
        {
            {
                Assert.That(() => new EmployeeNumber(input), Throws.InstanceOf<ContractException>());
            }
        }

        [TestCaseSource(nameof(ToStringCases))]
        public void ToString_ShouldCorrectlyConvert(DateTime startDate, int sequence, string expected)
        {
            EmployeeNumber number = new EmployeeNumber(startDate, sequence);
            string result = number.ToString();
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCaseSource(nameof(ToStringCases))]
        public void ImplicitConvertToString_ShouldCorrectlyConvert(DateTime startDate, int sequence, string expected)
        {
            EmployeeNumber number = new EmployeeNumber(startDate, sequence);
            string result = number;
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Dismiss_WithoutNotice_ShouldSetEndDateOnToday()
        {
            //Arrange
            Employee employee = new EmployeeBuilder().WithEndDate(null).Build();
            //Act
            employee.Dismiss(withNotice: false);
            //Assert
            Assert.That(employee.EndDate, Is.EqualTo(DateTime.Now).Within(10).Seconds);
        }

        [Test]
        public void Dismiss_WithoutNotice_EmployeeAlreadyHasEndDate_ShouldSetEndDateOnToday()
        {
            //Arrange
            Employee employee = new EmployeeBuilder().WithEndDate(DateTime.Now.AddDays(5)).Build();
            //Act
            employee.Dismiss(withNotice: false);
            //Assert
            Assert.That(employee.EndDate, Is.EqualTo(DateTime.Now).Within(10).Seconds);
        }

        [Test]
        public void Dismiss_WithNotice_EmployeeAlreadyHasEndDate_ShouldThrowContractException()
        {
            //Arrange
            Employee employee = new EmployeeBuilder().WithEndDate(DateTime.Now.AddDays(5)).Build();
            //Act + Assert
            Assert.That(() => employee.Dismiss(withNotice: true), Throws.InstanceOf<ContractException>());
        }

        [Test]
        public void Dismiss_WithNotice_LessThan3MonthsInService_ShouldSetEndDateInOneWeek()
        {
            //Arrange
            DateTime lessThan3MonthsAgo = DateTime.Now.AddDays(-28);
            Employee employee = new EmployeeBuilder()
                .WithStartDate(lessThan3MonthsAgo)
                .WithEndDate(null)
                .Build();
            //Act
            employee.Dismiss(withNotice: true);
            //Assert
            DateTime over1Week = DateTime.Now.AddDays(7);
            Assert.That(employee.EndDate, Is.EqualTo(over1Week).Within(10).Seconds);
        }

        [Test]
        public void Dismiss_WithNotice_LessThan12MonthsInService_ShouldSetEndDateIn2Weeks()
        {
            //Arrange
            DateTime lessThan12MonthsAgo = DateTime.Now.AddMonths(-10);
            Employee employee = new EmployeeBuilder().WithStartDate(lessThan12MonthsAgo).WithEndDate(null).Build();
            //Act
            employee.Dismiss(withNotice: true);
            //Assert
            DateTime over2Weeks = DateTime.Now.AddDays(14);
            Assert.That(employee.EndDate, Is.EqualTo(over2Weeks).Within(10).Seconds);
        }

        [Test]
        public void Dismiss_WithNotice_MoreThan12MonthsInService_ShouldSetEndDateIn4Weeks()
        {
            //Arrange
            DateTime moreThan12MonthsAgo = DateTime.Now.AddYears(-1);
            Employee employee = new EmployeeBuilder()
                .WithStartDate(moreThan12MonthsAgo)
                .WithEndDate(null)
                .Build();
            //Act
            employee.Dismiss(withNotice: true);
            //Assert
            DateTime over4Weeks = DateTime.Now.AddDays(28);
            Assert.That(employee.EndDate, Is.EqualTo(over4Weeks).Within(10).Seconds);
        }

        private static object[] ToStringCases =
        {
            new object[] { new DateTime(2000,12,29), 987, "20001229987" },
            new object[] { new DateTime(1,2,3), 4, "00010203004" }
        };
    }
}