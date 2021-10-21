using DevOps.Domain;
using DevOps.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevOps.Logic
{
    class DeveloperRepository : IDeveloperRepository
    {
        private readonly DevOpsContext _devOpsContext;

        public DeveloperRepository(DevOpsContext devOpsContext)
        {
            _devOpsContext = devOpsContext;
        }

        public async Task CommitTrackedChangesAsync()
        {
            await _devOpsContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Developer>> FindDevelopersWithoutATeamAsync()
        {
            return await Task.FromResult((IReadOnlyList<Developer>) _devOpsContext.Developers.Where(d => d.TeamId == Guid.Empty));
        }
    }
}
