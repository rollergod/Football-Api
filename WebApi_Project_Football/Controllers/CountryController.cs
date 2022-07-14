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
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        public CountryController(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(IEnumerable<Country>))]
        [ProducesResponseType(400)]
        public IActionResult GetCountries()
        {
            var countries = _countryRepository.GetCountries();

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

            if (!_countryRepository.CountryExist(id))
                return NotFound();

            var country = _countryRepository.GetCountry(id);

            return Ok(country);
        }

        [HttpGet("{countryId}/leagues")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetLeaguesFromCountry(int countryId)
        {
            if (countryId < 0)
                return BadRequest();

            if (!_countryRepository.CountryExist(countryId))
                return NotFound();

            var leagues = _countryRepository.GetLeaguesFromCountry(countryId);


            return Ok(leagues);

        }

    }
}
