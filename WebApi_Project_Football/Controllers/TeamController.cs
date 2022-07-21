using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi_Project_Football.Dto;
using WebApi_Project_Football.Interfaces;
using WebApi_Project_Football.Models;

namespace WebApi_Project_Football.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ILeagueRepository _leagueRepository;
        private readonly IMapper _mapper;
        public TeamController(ITeamRepository teamRepository,
                              ILeagueRepository leagueRepository,
                              IMapper mapper)
        {
            _teamRepository = teamRepository;
            _leagueRepository = leagueRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Team>))]
        [ProducesResponseType(400)]
        public IActionResult GetTeams()
        {
            var teams = _mapper.Map<List<TeamDto>>(_teamRepository.GetTeams());

            if (teams == null)
                return BadRequest();

            return Ok(teams);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Team))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetTeamById(int id)
        {
            if (id < 0)
                return BadRequest();

            if (!_teamRepository.TeamExists(id))
                return NotFound();

            var team = _mapper.Map<TeamDto>(_teamRepository.GetTeamById(id));

            return Ok(team);
        }

        [HttpGet("players/{teamId}")]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Player>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPlayersFromTeam(int teamId)
        {
            if (teamId < 0)
                return BadRequest();

            if (!_teamRepository.TeamExists(teamId))
                return NotFound();

            var players = _mapper.Map<List<PlayerDto>>(_teamRepository.GetPlayersFromTeam(teamId));

            return Ok(players);
        }

        [HttpGet("league/{teamId}")]
        [ProducesResponseType(200, Type = typeof(League))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetLeagueFromTeam(int teamId)
        {
            if (teamId < 0)
                return BadRequest();

            if (!_teamRepository.TeamExists(teamId))
                return NotFound();

            var league = _mapper.Map<LeagueDto>(_teamRepository.GetLeagueFromTeam(teamId));

            return Ok(league);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateTeam([FromQuery] int leagueId, [FromBody] TeamDto createdTeam)
        {
            if (createdTeam == null)
                return BadRequest();

            var team = _teamRepository.GetTeams()
                .Where(t => t.TeamName.Trim().ToLower() == createdTeam.TeamName.TrimEnd().ToLower())
                .FirstOrDefault();

            if(team != null)
            {
                ModelState.AddModelError("", "Team уже существует");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();

            var teamMap = _mapper.Map<Team>(createdTeam);
            teamMap.League = _leagueRepository.GetLeague(leagueId);

            if(!_teamRepository.CreateTeam(teamMap))
            {
                ModelState.AddModelError("", "Что-то пошло не так во время сохранения");
                return StatusCode(500, ModelState);
            }

            return Ok("Создано успешно");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateTeam(int id,
                                        [FromQuery] int leagueId,
                                        [FromBody] TeamDto updatedTeam)
        {
            if (updatedTeam == null)
                return BadRequest(ModelState);

            if (id != updatedTeam.Id)
                return BadRequest(ModelState);

            if (!_teamRepository.TeamExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var teamMap = _mapper.Map<Team>(updatedTeam);
            teamMap.League = _leagueRepository.GetLeague(leagueId);

            if(!_teamRepository.UpdateTeam(teamMap))
            {
                ModelState.AddModelError("", "Что-то пошло не так во время сохранения");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteTeam(int id)
        {
            if (id < 0)
                return BadRequest();

            if (!_teamRepository.TeamExists(id))
                return NotFound();

            var team = _teamRepository.GetTeamById(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_teamRepository.DeleteTeam(team))
                ModelState.AddModelError("", "Что-то пошло не так во время удаления Team");

            return NoContent();
        }
    }
}
