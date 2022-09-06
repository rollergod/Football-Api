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
        private readonly Mock<ICountryRepository> mock = new Mock<ICountryRepository>();
        [Fact]
        public void GetCountriesDtoFromRepository()
        {
            //Arrange
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
            mock.Setup(repo => repo.GetCountry(id)).Returns(GetCountries().FirstOrDefault(country => country.Id == id));
            mock.Setup(repo => repo.CountryExists(id)).Returns(GetCountries().Any(country => country.Id == id));
            _mapper.Setup(m => m.Map<CountryDto>(mock.Object.GetCountry(id))).Returns(GetCountryById()); 
            var controller = new CountryController(mock.Object, _mapper.Object);

            var result = controller.GetCountry(id) as OkObjectResult;


            var country = result.Value as CountryDto;
            Assert.Equal("Russia", country.CountryName);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void GetMethodReturnsNotFound()
        {
            int id = 0;
            mock.Setup(repo => repo.CountryExists(id)).Returns(GetCountries().Any(country => country.Id == id));
            _mapper.Setup(m => m.Map<CountryDto>(mock.Object.GetCountry(id))).Returns(GetCountryById());
            var controller = new CountryController(mock.Object, _mapper.Object);

            var result = controller.GetCountry(id) as StatusCodeResult;

            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void PostCountryMethod_ReturnsOk()
        {
            CountryDto newCountry = new CountryDto { Id = 3, CountryName = "Japan" };
            Country Country = new Country { Id = 3, CountryName = "Japan" };
            mock.Setup(repo => repo.GetCountries()).Returns(GetCountries());
            mock.Setup(repo => repo.CreateContry(It.IsAny<Country>())).Returns(true);
            _mapper.Setup(m => m.Map<Country>(newCountry)).Returns(Country);
            var controller = new CountryController(mock.Object, _mapper.Object);

            var result = controller.CreateCountry(newCountry) as ObjectResult;

            Assert.Equal("Создано успешно", result.Value);
        }

        [Fact]
        public void PostCountryMethod_ReturnsBadRequest()
        {
            CountryDto newCountryDto = new CountryDto { Id = 4, CountryName = "USA" };
            mock.Setup(repo => repo.GetCountries()).Returns(GetCountries());
            var controller = new CountryController(mock.Object, _mapper.Object);

            var result = controller.CreateCountry(newCountryDto) as ObjectResult;

            Assert.Equal(422, result.StatusCode);
        }

        [Fact]
        public void UpdateCountry_ReturnsBadRequestWhenCountryIsNull()
        {
            var controller = new CountryController(mock.Object, _mapper.Object);

            var result = controller.UpdateCountry(0, null);

            var objectResult = result as ObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, objectResult.StatusCode);
        }

        [Fact]
        public void UpdateCountry_ReturnsBadRequestWhenIdIsNotEqualCountryId()
        {
            int testId = 0;
            var controller = new CountryController(mock.Object, _mapper.Object);

            var result = controller.UpdateCountry(testId, new CountryDto { Id = 1, CountryName = "TestCountry" });

            var objectResult = result as ObjectResult;
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, objectResult.StatusCode);
        }

        [Fact]
        public void UpdateCountry_ReturnsNotFoundWithIdIs0()
        {
            int testId = 5;
            mock.Setup(repo => repo.GetCountries()).Returns(GetCountries());
            mock.Setup(repo => repo.CountryExists(testId))
                                        .Returns(GetCountries()
                                        .Any(country => country.Id == testId));
            var controller = new CountryController(mock.Object,_mapper.Object);

            var result = controller.UpdateCountry(testId, new CountryDto { Id = 5,CountryName = "TestCountry"});

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void UpdateCountry_ReturnsStatusCode500()
        {
            int testId = 1;
            var updateCountryDto = new CountryDto() { Id = 1, CountryName = "TestCountry" };
            mock.Setup(repo => repo.GetCountries()).Returns(GetCountries());
            mock.Setup(repo => repo.UpdateCountry(new Country { Id = 1, CountryName = "TestCountry" })).Returns(false);
            mock.Setup(repo => repo.CountryExists(testId)).Returns(true);
            var controller = new CountryController(mock.Object, _mapper.Object);

            var result = controller.UpdateCountry(1, updateCountryDto) as ObjectResult;

            Assert.Equal(500, result.StatusCode);
        }


        [Fact]
        public void UpdateCountry_ReturnsNoContent()
        {
            int testId = 1;
            var updateCountryDto = new CountryDto() { Id = 1, CountryName = "TestCountry" };
            Country updateCountry = new Country() { Id = 1, CountryName = "TestCountry" };
            _mapper.Setup(m => m.Map<Country>(updateCountryDto)).Returns(updateCountry);
            mock.Setup(repo => repo.GetCountries()).Returns(GetCountries());
            mock.Setup(repo => repo.UpdateCountry(updateCountry)).Returns(true);
            mock.Setup(repo => repo.CountryExists(testId)).Returns(true);
            var controller = new CountryController(mock.Object, _mapper.Object);
            
            var result = controller.UpdateCountry(1,updateCountryDto);
        }

        [Fact]
        public void DeleteCountry_ReturnsBadRequestWhenIdLessThan0()
        {
            int testId = -1;
            var controller = new CountryController(mock.Object, _mapper.Object);

            var result = controller.DeleteCountry(testId);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void DeleteCountry_ReturnsNotFound()
        {
            int testId = 5;
            mock.Setup(repo => repo.GetCountries()).Returns(GetCountries());
            mock.Setup(repo => repo.CountryExists(testId))
                                        .Returns(GetCountries()
                                        .Any(country => country.Id == testId));
            var controller = new CountryController(mock.Object, _mapper.Object);

            var result = controller.DeleteCountry(testId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteCountry_ReturnsStatusCode500()
        {
            int testId = 1;
            mock.Setup(repo => repo.GetCountries()).Returns(GetCountries());
            mock.Setup(repo => repo.CountryExists(testId))
                                        .Returns(GetCountries()
                                        .Any(country => country.Id == testId));
            mock.Setup(repo => repo.GetCountry(testId))
                                        .Returns(GetCountries()
                                        .FirstOrDefault(country => country.Id == testId));
            mock.Setup(repo => repo.DeleteCountry(It.IsAny<Country>())).Returns(false);
            var controller = new CountryController(mock.Object, _mapper.Object);

            var result = controller.DeleteCountry(testId);

            var statusCode = result as ObjectResult;
            Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCode.StatusCode);
        }

        [Fact]
        public void DeleteCountry_ReturnsNoContent()
        {
            int testId = 1;
            mock.Setup(repo => repo.GetCountries()).Returns(GetCountries());
            mock.Setup(repo => repo.CountryExists(testId))
                                        .Returns(GetCountries()
                                        .Any(country => country.Id == testId));
            mock.Setup(repo => repo.GetCountry(testId))
                                        .Returns(GetCountries()
                                        .FirstOrDefault(country => country.Id == testId));
            var controller = new CountryController(mock.Object, _mapper.Object);

            var result = controller.DeleteCountry(testId);

            Assert.IsType<NoContentResult>(result);
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
                new Country {Id = 2, CountryName = "Mexico" },
                new Country { Id = 4, CountryName = "USA" },
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
