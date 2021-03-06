using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace HumanRelations.Domain
{
    internal class Employee : Entity, IEmployee
    {
        private const int THREE_MONTH_NOTICE = 7;
        private const int TWELVE_MONTH_NOTICE = 14;
        private const int TWELVE_MONTH_PLUS_NOTICE = 28;
        public EmployeeNumber Number { get; private set; }
        public string LastName { get; private set; }
        public string FirstName { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }

        private Employee() { }

        protected override IEnumerable<object> GetIdComponents()
        {
            yield return Number;
        }

        public void Dismiss(bool withNotice = true)
        {
            if (withNotice == false)
            {
                this.EndDate = DateTime.Now;
            }

            if (withNotice == true)
            {
                if (EndDate != null) { throw new ContractException(); }
                if (this.StartDate.AddMonths(3) > DateTime.Now)
                {
                    this.EndDate = DateTime.Now.AddDays(THREE_MONTH_NOTICE);
                }
                else if (this.StartDate.AddYears(1) > DateTime.Now)
                {
                    this.EndDate = DateTime.Now.AddDays(TWELVE_MONTH_NOTICE);
                }
                else if (this.StartDate.AddYears(1) < DateTime.Now)
                {
                    this.EndDate = DateTime.Now.AddDays(TWELVE_MONTH_PLUS_NOTICE);
                }
            }
        }

        internal class Factory : IEmployeeFactory
        {
            public IEmployee CreateNew(string lastName, string firstName, DateTime startDate, int sequence)
            {
                Contracts.Require(startDate >= DateTime.Now.AddYears(-1), "The start date of an employee cannot be more than 1 year in the past");
                Contracts.Require(!string.IsNullOrEmpty(lastName), "The last name of an employee cannot be empty");
                Contracts.Require(lastName.Length >= 2, "The last name of an employee must at least have 2 characters");
                Contracts.Require(!string.IsNullOrEmpty(firstName), "The first name of an employee cannot be empty");
                Contracts.Require(firstName.Length >= 2, "The first name of an employee must at least have 2 characters");

                var employee = new Employee
                {
                    Number = new EmployeeNumber(startDate, sequence),
                    FirstName = firstName,
                    LastName = lastName,
                    StartDate = startDate,
                    EndDate = null
                };
                return employee;
            }
        }
    }
}
