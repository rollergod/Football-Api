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

        [HttpGet("player/{statId}")]
        [ProducesResponseType(200, Type = typeof(Player))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPlayerFromStat(int statId)
        {
            if (statId < 0)
                return BadRequest();

            if (!_statisticRepository.StatisticExists(statId))
                return NotFound();

            var player = _mapper.Map<PlayerDto>(_statisticRepository.GetPlayerFromStatistic(statId));

            return Ok(player);
        } 

    }
}
