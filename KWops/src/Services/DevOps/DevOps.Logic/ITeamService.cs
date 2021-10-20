using System.Threading.Tasks;
using DevOps.Domain;
namespace DevOps.Logic
{
    public interface ITeamService
    {
        Task AssembleDevelopersAsyncFor(Team team, int requiredNumberOfDevelopers);
    }
}
