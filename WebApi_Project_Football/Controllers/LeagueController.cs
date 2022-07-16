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
    public class LeagueController : ControllerBase
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ILeagueRepository _leagueRepository;

        public LeagueController(ILeagueRepository leagueRepository, ITeamRepository teamRepository)
        {
            _leagueRepository = leagueRepository;
            _teamRepository = teamRepository;
        }

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(IEnumerable<League>))]
        [ProducesResponseType(400)]
        public IActionResult GetLeagues()
        {
            var leagues = _leagueRepository.GetLeagues();

            if (leagues == null)
                return BadRequest();

            return Ok(leagues);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(League))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetLeagueById(int id)
        {
            if (id < 0)
                return BadRequest();

            if (!_leagueRepository.LeagueExists(id))
                return NotFound();

            var league = _leagueRepository.GetLeague(id);

            return Ok(league);
        }

        [HttpGet("league/{teamId}")]
        [ProducesResponseType(200,Type = typeof(League))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetLeagueFromTeam(int teamId)
        {

            if (teamId < 0)
                return BadRequest();

            if (!_teamRepository.TeamExists(teamId))
                return NotFound();

            var league = _leagueRepository.GetLeagueFromTeam(teamId);

            return Ok(league);
        }

        [HttpGet("teams/{leagueId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Team>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetTeamsFromLeague(int leagueId)
        {
            if (leagueId < 0)
                return BadRequest();

            if (!_leagueRepository.LeagueExists(leagueId))
                return NotFound();

            var teams = _leagueRepository.GetTeamsFromLeague(leagueId);

            return Ok(teams);
        }
    }
}
