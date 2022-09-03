using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WebApi_Project_Football.Controllers;
using WebApi_Project_Football.Dto;
using WebApi_Project_Football.Interfaces;
using WebApi_Project_Football.Models;
using Xunit;

namespace WebApi_Project_Football.Tests
{
    public class CountryRepositoryTests
    {
        //private ICountryRepository _countryRepository;
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        [Fact]
        public void GetCountriesDtoFromRepository()
        {
            //Arrange
            var mock = new Mock<ICountryRepository>();
            mock.Setup(repo => repo.GetCountries()).Returns(GetCountries());
            _mapper.Setup(m => m.Map<List<CountryDto>>(mock.Object.GetCountries())).Returns(GetCountriesDto());
            var controller = new CountryController(mock.Object,_mapper.Object);

            //Act
            var result = controller.GetCountries() as OkObjectResult;


            //Assert
            var list = result.Value as List<CountryDto>;
            Assert.Equal(200,result.StatusCode);
            Assert.Equal(GetCountriesDto().Count, list.Count);
            Assert.Equal("Russia", list[0].CountryName);
        }
        
        [Fact]
        public void GetCountryById1()
        {
            int id = 1;
            var mock = new Mock<ICountryRepository>();
            mock.Setup(repo => repo.GetCountry(id)).Returns(GetCountries().FirstOrDefault(country => country.Id == id));
            mock.Setup(repo => repo.CountryExists(id)).Returns(true);
            _mapper.Setup(m => m.Map<CountryDto>(mock.Object.GetCountry(id))).Returns(GetCountryById()); 
            var controller = new CountryController(mock.Object, _mapper.Object);

            var result = controller.GetCountry(id) as OkObjectResult;


            var country = result.Value as CountryDto;
            Assert.Equal("Russia", country.CountryName);
            Assert.Equal(200, result.StatusCode);

        }

        private CountryDto GetCountryById()
        {
            return new CountryDto { Id = 1, CountryName = "Russia" };
        }
        private List<Country> GetCountries()
        {
            var countries = new List<Country>
            {
                new Country {Id = 1, CountryName = "Russia" },
                new Country {Id = 2, CountryName = "Mexico" }
            };
            return countries;
        }
        private List<CountryDto> GetCountriesDto()
        {
            var countries = new List<CountryDto>
            {
                new CountryDto {Id = 1, CountryName = "Russia" },
                new CountryDto {Id = 2, CountryName = "Mexico" }
            };
            return countries;
        }
    }
}
