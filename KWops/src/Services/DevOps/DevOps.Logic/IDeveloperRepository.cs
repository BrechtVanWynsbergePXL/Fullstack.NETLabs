using System.Collections.Generic;
using System.Threading.Tasks;
using DevOps.Domain;
namespace DevOps.Logic
{
    public interface IDeveloperRepository
    {
        Task<IReadOnlyList<Developer>> FindDevelopersWithoutATeamAsync();
        Task CommitTrackedChangesAsync();
        Task<Developer> GetByIdAsync(string id);
        Task AddAsync(Developer newDeveloper);
    }
}
