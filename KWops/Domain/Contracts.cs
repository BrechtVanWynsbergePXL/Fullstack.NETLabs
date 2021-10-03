using System.Diagnostics;

namespace Domain
{
    public static class Contracts
    {
        [DebuggerStepThrough]
        public static void Require(bool precondition, string message = "")
        {
            if (!precondition)
            {
                throw new ContractException(message);
            }
        }
    }
}
