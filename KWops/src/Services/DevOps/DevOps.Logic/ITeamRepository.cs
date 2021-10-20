using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevOps.Domain;
namespace DevOps.Logic
{
    public interface ITeamRepository
    {
        Task<IReadOnlyList<Team>> GetAllAsync();
        Task<Team> GetByIdAsync(Guid teamId);
    }
}
