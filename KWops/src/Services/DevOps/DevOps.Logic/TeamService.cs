using DevOps.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DevOps.Logic
{
    internal class TeamService : ITeamService
    {
        private readonly IDeveloperRepository _developerRepository;

        public TeamService(IDeveloperRepository developerRepository)
        {
            _developerRepository = developerRepository;
        }

        public async Task AssembleDevelopersAsyncFor(Team team, int requiredNumberOfDevelopers)
        {
            IReadOnlyList<Developer> devList = await _developerRepository.FindDevelopersWithoutATeamAsync();
            if (devList.Count < requiredNumberOfDevelopers)
            {
                foreach (var developer in devList)
                {
                    team.Join(developer);
                }
                await _developerRepository.CommitTrackedChangesAsync();
                return;
            }

            for (int i = 0; i < requiredNumberOfDevelopers; i++)
            {
                team.Join(devList[i]);
            }
            await _developerRepository.CommitTrackedChangesAsync();
            return;
        }
    }
}
