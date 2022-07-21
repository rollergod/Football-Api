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
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        [ProducesResponseType(400)]
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());

            if (countries == null)
                return BadRequest();

            return Ok(countries);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetCountry(int id)
        {
            if (id < 0)
                return BadRequest();

            if (!_countryRepository.CountryExists(id))
                return NotFound();

            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(id));

            return Ok(country);
        }

        [HttpGet("{countryId}/leagues")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<League>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetLeaguesFromCountry(int countryId)
        {
            if (countryId < 0)
                return BadRequest();

            if (!_countryRepository.CountryExists(countryId))
                return NotFound();

            var leagues = _mapper.Map<List<LeagueDto>>(_countryRepository.GetLeaguesFromCountry(countryId));


            return Ok(leagues);

        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] CountryDto countryCreated)
        {
            if (countryCreated == null)
                return BadRequest();

            var country = _countryRepository.GetCountries()
                .Where(c => c.CountryName.Trim().ToLower() == countryCreated.CountryName.TrimEnd().ToLower())
                .FirstOrDefault(); // check exists

            if (country != null)
            {
                ModelState.AddModelError("", "Country уже существует");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryMap = _mapper.Map<Country>(countryCreated);

            if (!_countryRepository.CreateContry(countryMap))
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
        public IActionResult UpdateCountry(int id, [FromBody] CountryDto updatedCountry)
        {
            if (updatedCountry == null)
                return BadRequest(ModelState);

            if (id != updatedCountry.Id)
                return BadRequest(ModelState);

            if (!_countryRepository.CountryExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var countryMap = _mapper.Map<Country>(updatedCountry);

            if(!_countryRepository.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("", "Что-то пошло не так во время обновления");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCountry(int id)
        {
            if (id < 0)
                return BadRequest();

            if (!_countryRepository.CountryExists(id))
                return NotFound();

            var country = _countryRepository.GetCountry(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_countryRepository.DeleteCountry(country))
                ModelState.AddModelError("", "Что-то пошло не так во время удаления Country");

            return NoContent();
                
        }
    }
}
