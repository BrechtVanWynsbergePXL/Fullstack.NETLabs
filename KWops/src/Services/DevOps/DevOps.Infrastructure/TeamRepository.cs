using DevOps.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevOps.Logic
{
    class TeamRepository : ITeamRepository
    {
        private readonly DevOpsContext _devOpsContext;

        public TeamRepository(DevOpsContext devOpsContext)
        {
            _devOpsContext = devOpsContext;
        }

        public async Task<IReadOnlyList<Team>> GetAllAsync()
        {
            return await Task.FromResult(_devOpsContext.Teams.Select(t => t).ToList());
            //return (IReadOnlyList<Team>)await Task.FromResult(_devOpsContext.Teams.Select(t => t.Developers).ToList());
        }

        public async Task<Team> GetByIdAsync(Guid teamId)
        {
            return await _devOpsContext.Teams.FindAsync(teamId);
        }
    }
}
