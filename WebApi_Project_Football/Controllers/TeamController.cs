using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi_Project_Football.Interfaces;
using WebApi_Project_Football.Models;

namespace WebApi_Project_Football.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamController : ControllerBase
    {
        private readonly ITeamRepository _teamRepository;

        public TeamController(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Team>))]
        [ProducesResponseType(400)]
        public IActionResult GetTeams()
        {
            var teams = _teamRepository.GetTeams();

            if (teams == null)
                return BadRequest();

            return Ok(teams);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Team>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetTeamById(int id)
        {
            if (id < 0)
                return BadRequest();

            if (!_teamRepository.TeamExists(id))
                return NotFound();

            var team = _teamRepository.GetTeamById(id);

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

            var players = _teamRepository.GetPlayersFromTeam(teamId);

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

            var league = _teamRepository.GetLeagueFromTeam(teamId);

            return Ok(league);
        }
    }
}
