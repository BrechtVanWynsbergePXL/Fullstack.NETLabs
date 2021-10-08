using System;
using System.Collections.Generic;
using System.Text;

namespace HumanRelations.Domain
{
    public interface IEmployee
    {
        EmployeeNumber Number { get; }
        string LastName { get; }
        string FirstName { get; }
        DateTime StartDate { get; }
        DateTime? EndDate { get; }
        void Dismiss(bool withNotice = true);
    }
}
