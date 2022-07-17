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
    public class StatisticController : ControllerBase
    {
        private readonly IStatisticRepository _statisticRepository;
        private readonly IMapper _mapper;
        public StatisticController(IStatisticRepository statisticRepository, IMapper mapper)
        {
            _statisticRepository = statisticRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Statistic>))]
        [ProducesResponseType(400)]
        public IActionResult GetStats()
        {
            var statistics = _mapper.Map<List<StatisticDto>>(_statisticRepository.GetStatistics());

            if (statistics == null)
                return BadRequest();

            return Ok(statistics);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200,Type = typeof(Statistic))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetStatById(int id)
        {
            if (id < 0)
                return BadRequest();

            if (!_statisticRepository.StatisticExists(id))
                return NotFound();

            var stat = _mapper.Map<StatisticDto>(_statisticRepository.GetStatistic(id));

            return Ok(stat);
        } 

        [HttpGet("players/{statId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Player>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPlayerFromStat(int statId)
        {
            if (statId < 0)
                return BadRequest();

            if (!_statisticRepository.StatisticExists(statId))
                return NotFound();

            var players = _mapper.Map<List<PlayerDto>>(_statisticRepository.GetPlayersFromStatistic(statId));

            return Ok(players);
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateStatistic([FromBody] StatisticDto createdStat)
        {
            if (createdStat == null)
                return BadRequest();

            var statistic = _statisticRepository.GetStatistics()
                .FirstOrDefault(s => (s.GamesPlayed == createdStat.GamesPlayed)
                && (s.MinutesPlayed == createdStat.MinutesPlayed)
                && (s.Goals == createdStat.Goals)
                && (s.Assists == createdStat.Assists)
                &&(s.YellowCards == createdStat.YellowCards)
                &&(s.RedCards == createdStat.RedCards));

            if (statistic != null)
            {
                ModelState.AddModelError("", "Statistic уже существует");
                return StatusCode(522, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();

            var statMap = _mapper.Map<Statistic>(createdStat);

            if(!_statisticRepository.CreateStatistic(statMap))
            {
                ModelState.AddModelError("", "Что-то пошло не так во время сохранения");
                return StatusCode(500, ModelState);
            }

            return Ok("Создано успешно");

        }
    }
}
