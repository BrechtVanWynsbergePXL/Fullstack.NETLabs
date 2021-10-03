using System;
using System.Collections.Generic;
using System.Text;

namespace HumanRelations.Domain
{
    public interface IEmployeeFactory
    {
        IEmployee CreateNew(string lastName, string firstName, DateTime startDate, int sequence);
    }
}
