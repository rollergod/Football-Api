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
    public class LeagueRepositoryTests
    {
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<ILeagueRepository> _leagueRepositoryMock = new Mock<ILeagueRepository>();
        private readonly Mock<ICountryRepository> _countryRepositoryMock = new Mock<ICountryRepository>();
        private readonly Mock<ITeamRepository> _teamRepositoryMock = new Mock<ITeamRepository>();
        
        [Fact]
        public void GetLeagues_ReturnLeagues()
        {
            _leagueRepositoryMock.Setup(repo => repo.GetLeagues()).Returns(GetLeagues());
            _mapper.Setup(m => m.Map<List<LeagueDto>>(It.IsAny<List<League>>())).Returns(GetLeaguesDto());
            var controller = new LeagueController(
                                _leagueRepositoryMock.Object,
                                _teamRepositoryMock.Object,
                                _mapper.Object,
                                _countryRepositoryMock.Object);

            var result = controller.GetLeagues() as OkObjectResult;

            var list = result.Value as List<LeagueDto>;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("RPL", list[0].LeagueName);
        }

        [Fact]
        public void GetLeagueById_ReturnBadRequest()
        {
            int testId = -1;
            var controller = new LeagueController(
                                _leagueRepositoryMock.Object,
                                _teamRepositoryMock.Object,
                                _mapper.Object,
                                _countryRepositoryMock.Object);

            var result = controller.GetLeagueById(testId) as BadRequestResult;

            Assert.IsType<BadRequestResult>(result);
            Assert.Equal(400,result.StatusCode);
        }

        [Fact]
        public void GetLeagueById_ReturnNotFound()
        {
            int testId = 5;
            _leagueRepositoryMock.Setup(repo => repo.LeagueExists(testId)).Returns(GetLeagues().Any(league => league.Id == testId));
            var controller = new LeagueController(
                                 _leagueRepositoryMock.Object,
                                 _teamRepositoryMock.Object,
                                 _mapper.Object,
                                 _countryRepositoryMock.Object);

            var result = controller.GetLeagueById(testId) as NotFoundResult;

            Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public void GetLeagueById_ReturnLeague()
        {
            int testId = 1;
            _leagueRepositoryMock.Setup(repo => repo.LeagueExists(testId)).Returns(true);
            _mapper.Setup(m => m.Map<LeagueDto>(It.IsAny<League>()))
                .Returns(GetLeaguesDto().FirstOrDefault(league => league.Id == testId));
            var controller = new LeagueController(
                             _leagueRepositoryMock.Object,
                             _teamRepositoryMock.Object,
                             _mapper.Object,
                             _countryRepositoryMock.Object);

            var result = controller.GetLeagueById(testId) as OkObjectResult;

            var league = result.Value as LeagueDto;
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(testId, league.Id);
            Assert.Equal("RPL", league.LeagueName);
        }


        private List<League> GetLeagues()
        {
            var collection = new List<League>() {
                new League() {Id = 1,LeagueName="RPL"},
                new League() {Id = 2,LeagueName="EPL"},
            };
            return collection;
        }

        private List<LeagueDto> GetLeaguesDto()
        {
            var collection = new List<LeagueDto>() {
                new LeagueDto() {Id = 1,LeagueName="RPL"},
                new LeagueDto() {Id = 2,LeagueName="EPL"},
            };
            return collection;
        }
    }
}
