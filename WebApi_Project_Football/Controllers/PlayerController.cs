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
        private readonly IMapper _mapper;
        public PlayerController(IPlayerRepository playerRepository, IMapper mapper)
        {
            _playerRepository = playerRepository;
            _mapper = mapper;
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
            players = _playerRepository.InitialTeamForPlayers(players).ToList();
          
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
        [ProducesResponseType(200, Type = typeof(Team))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetStatisticFromPlayer(int playerId)
        {
            if (playerId < 0)
                return BadRequest();

            if (!_playerRepository.PlayerExists(playerId))
                return NotFound();

            var stat = _mapper.Map<TeamDto>(_playerRepository.GetStatisticFromPlayer(playerId));

            return Ok(stat);
        }
    }
}
