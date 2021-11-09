using AutoMapper;
using DevOps.Api.Models;
using DevOps.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevOps.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private readonly ITeamRepository _teamRepository;
        private readonly IMapper _mapper;

        public TeamsController(ITeamService teamService, ITeamRepository teamRepository, IMapper mapper)
        {
            _teamService = teamService;
            _teamRepository = teamRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Team> teamList = (List<Team>)await _teamRepository.GetAllAsync();
            List<TeamDetailModel> mappedTeamList = new();
            foreach (var team in teamList)
            {
                var mappedTeam = _mapper.Map<TeamDetailModel>(team);
                mappedTeamList.Add(mappedTeam);
            }            
            return teamList == null ? NotFound() : Ok(mappedTeamList);
        }

        [HttpPost("{id}/assemble")]
        [Authorize(policy:"write")]
        public async Task<IActionResult> AssembleTeam(Guid id, TeamAssembleInputModel model)
        {
            Team team = await _teamRepository.GetByIdAsync(id);
            if(team == null)
            {
                return NotFound();
            }            
            await _teamService.AssembleDevelopersAsyncFor(team, model.RequiredNumberOfDevelopers);
            return Ok();
        }
    }
}
