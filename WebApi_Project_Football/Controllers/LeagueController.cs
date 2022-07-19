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
    public class LeagueController : ControllerBase
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ILeagueRepository _leagueRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public LeagueController(ILeagueRepository leagueRepository,
                                ITeamRepository teamRepository,
                                IMapper mapper, 
                                ICountryRepository countryRepository)
        {
            _leagueRepository = leagueRepository;
            _teamRepository = teamRepository;
            _mapper = mapper;
            _countryRepository = countryRepository;
        }

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(IEnumerable<League>))]
        [ProducesResponseType(400)]
        public IActionResult GetLeagues()
        {
            var leagues = _mapper.Map<List<LeagueDto>>(_leagueRepository.GetLeagues());

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

            var league = _mapper.Map<LeagueDto>(_leagueRepository.GetLeague(id));

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

            var league = _mapper.Map<LeagueDto>(_leagueRepository.GetLeagueFromTeam(teamId));

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

            var teams = _mapper.Map<List<TeamDto>>(_leagueRepository.GetTeamsFromLeague(leagueId));

            return Ok(teams);
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult CreateLeague([FromQuery] int countryId,[FromBody] LeagueDto createdLeague)
        {
            //if (!_countryRepository.CountryExists(countryId))
            //{
            //    //ModelState.AddModelError("", "Country не существует");
            //    //return StatusCode(422, ModelState);
            //    return NotFound("Country не существует");
            //}

            if (createdLeague == null)
                return BadRequest();

            var league = _leagueRepository.GetLeagues()
                .Where(l => l.LeagueName.Trim().ToLower() == createdLeague.LeagueName.TrimEnd().ToLower())
                .FirstOrDefault();

            if(league != null)
            {
                ModelState.AddModelError("", "League уже существует");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var leagueMap = _mapper.Map<League>(createdLeague);
            leagueMap.Country = _countryRepository.GetCountry(countryId);

            if(!_leagueRepository.CreateLeague(leagueMap))
            {
                ModelState.AddModelError("", "Что-то пошло не так во время сохранения");
                return StatusCode(500, ModelState);
            }

            return Ok("Создано успешно");
        }


        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateLeague(int id,
                                          [FromQuery] int countryId,
                                          [FromBody] LeagueDto updatedLeague)
        {
            if (updatedLeague == null)
                return BadRequest(ModelState);

            if (id != updatedLeague.Id)
                return BadRequest(ModelState);

            if (!_leagueRepository.LeagueExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var leagueMap = _mapper.Map<League>(updatedLeague);
            leagueMap.Country = _countryRepository.GetCountry(countryId);

            if(!_leagueRepository.UpdateLeague(leagueMap))
            {
                ModelState.AddModelError("", "Что-то пошло не так во время обновления");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
