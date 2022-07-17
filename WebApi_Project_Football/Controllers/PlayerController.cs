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
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IStatisticRepository _statRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public PlayerController(IPlayerRepository playerRepository,
                                IMapper mapper,
                                ITeamRepository teamRepository,
                                IStatisticRepository statRepository,
                                ICountryRepository countryRepository)
        {
            _playerRepository = playerRepository;
            _mapper = mapper;
            _teamRepository = teamRepository;
            _statRepository = statRepository;
            _countryRepository = countryRepository;
        }

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Player>))]
        [ProducesResponseType(400)]
        public IActionResult GetPlayers()
        {
            var players = _mapper.Map<List<PlayerDto>>(_playerRepository.GetPlayers());

            if (players == null)
                return BadRequest();

            //получаем имена клубов
            //players = _playerRepository.InitialTeamForPlayers(players).ToList();
          
            return Ok(players);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Player))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPlayerById(int id)
        {
            if (id < 0)
                return BadRequest();

            if (!_playerRepository.PlayerExists(id))
                return NotFound();

            var player = _mapper.Map<PlayerDto>(_playerRepository.GetPlayerById(id));

            return Ok(player);
        }

        [HttpGet("team/{playerId}")]
        [ProducesResponseType(200, Type = typeof(Team))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetTeamFromPlayer(int playerId)
        {
            if (playerId < 0)
                return BadRequest();

            if (!_playerRepository.PlayerExists(playerId))
                return NotFound();

            var team = _mapper.Map<TeamDto>(_playerRepository.GetTeamFromPlayer(playerId));

            return Ok(team);
        }

        [HttpGet("statistic/{playerId}")]
        [ProducesResponseType(200, Type = typeof(Statistic))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetStatisticFromPlayer(int playerId)
        {
            if (playerId < 0)
                return BadRequest();

            if (!_playerRepository.PlayerExists(playerId))
                return NotFound();

            var stat = _mapper.Map<StatisticDto>(_playerRepository.GetStatisticFromPlayer(playerId));

            return Ok(stat);
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult CreatePlayer([FromQuery] int countryId,
                                          [FromQuery] int teamId,
                                          [FromQuery] int statisticId,
                                          [FromBody] PlayerDto createdPlayer)
        {
            if(createdPlayer == null)
                return BadRequest();

            var player = _playerRepository.GetPlayers()
                .Where(p => p.FirstName.Trim().ToLower() == createdPlayer.FirstName.TrimEnd().ToLower())
                .FirstOrDefault();

            if(player != null)
            {
                ModelState.AddModelError("", "Player уже существует");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();

            var playerMap = _mapper.Map<Player>(createdPlayer);

            playerMap.Country = _countryRepository.GetCountry(countryId);

            playerMap.Team = _teamRepository.GetTeamById(teamId);

            playerMap.Statistic = _statRepository.GetStatistic(statisticId);

            if(!_playerRepository.CreatePlayer(playerMap))
            {
                ModelState.AddModelError("", "Что-то пошло не так во время сохранения");
                return StatusCode(500, ModelState);
            }

            return Ok("Создано успешно");
        }
    }
}
