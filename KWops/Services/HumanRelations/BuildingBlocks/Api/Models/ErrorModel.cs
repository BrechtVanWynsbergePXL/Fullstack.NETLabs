using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanRelations.API.Models
{
    public class ErrorModel
    {
        public string Message { get; set; }

        public ErrorModel(string message)
        {
            Message = message;
        }
    }
}
