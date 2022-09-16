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
            _leagueRepositoryMock.Setup(repo => repo.GetLeagues())
                .Returns(GetLeagues());
            _mapper.Setup(m => m.Map<List<LeagueDto>>(It.IsAny<List<League>>()))
                .Returns(GetLeaguesDto());
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
            _leagueRepositoryMock.Setup(repo => repo.LeagueExists(testId))
                .Returns(GetLeagues().Any(league => league.Id == testId));
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
            _leagueRepositoryMock.Setup(repo => repo.LeagueExists(testId))
                .Returns(true);
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

        [Fact]
        public void GetLeagueByTeamId_ReturnsBadRequest()
        {
            int testId = -1;
            _leagueRepositoryMock.Setup(mock => mock.GetLeagueFromTeam(testId));
            var controller = new LeagueController(
                             _leagueRepositoryMock.Object,
                             _teamRepositoryMock.Object,
                             _mapper.Object,
                             _countryRepositoryMock.Object);

            var result = controller.GetLeagueFromTeam(testId) as ActionResult;

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void GetLeagueByTeamId_ReturnsNotFound()
        {
            int testId = 3;
            _teamRepositoryMock.Setup(mock => mock.TeamExists(testId))
                .Returns(GetLeagues().Any(league => league.Id == testId));
            _leagueRepositoryMock.Setup(mock => mock.GetLeagueFromTeam(testId));
            var controller = new LeagueController(
                             _leagueRepositoryMock.Object,
                             _teamRepositoryMock.Object,
                             _mapper.Object,
                             _countryRepositoryMock.Object);

            var result = controller.GetLeagueFromTeam(testId) as ActionResult;

            Assert.IsType<NotFoundResult>(result);
        }
          
        [Fact]
        public void GetLeague_ReturnsStatusCode200WithLeague()
        {
            int testId = 1;
            _teamRepositoryMock.Setup(mock => mock.TeamExists(testId))
                .Returns(true);
            _leagueRepositoryMock.Setup(mock => mock.GetLeagueFromTeam(testId))
                .Returns(GetLeagues().FirstOrDefault(league => league.Id == testId));
            _mapper.Setup(m => m.Map<LeagueDto>(_leagueRepositoryMock.Object
                .GetLeagueFromTeam(testId)))
                .Returns(GetLeaguesDto().FirstOrDefault(league => league.Id == testId));
            var controller = new LeagueController(
                            _leagueRepositoryMock.Object,
                            _teamRepositoryMock.Object,
                            _mapper.Object,
                            _countryRepositoryMock.Object);

            var result = controller.GetLeagueFromTeam(testId) as ActionResult;


            var resultValue = result as OkObjectResult;
            var league = (LeagueDto)resultValue.Value;
            Assert.Equal(200,resultValue.StatusCode);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("RPL", league.LeagueName);
        }

        [Fact]
        public void CreateLeague_ReturnsBadRequestWhenLeagueIsNull()
        {
            _leagueRepositoryMock.Setup(mock => mock.CreateLeague(null));
            var controller = new LeagueController(
                            _leagueRepositoryMock.Object,
                            _teamRepositoryMock.Object,
                            _mapper.Object,
                            _countryRepositoryMock.Object);

            var result = controller.CreateLeague(0,null) as ActionResult;

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void CreateLeague_LeagueAlreadyExistsWithSameName()
        {
            var createdLeague = new LeagueDto() { Id = 3, LeagueName = "RPL" };
            _leagueRepositoryMock.Setup(mock => mock.GetLeagues())
                .Returns(GetLeagues());
            var controller = new LeagueController(
                           _leagueRepositoryMock.Object,
                           _teamRepositoryMock.Object,
                           _mapper.Object,
                           _countryRepositoryMock.Object);

            var result = controller.CreateLeague(0, createdLeague) as ActionResult;

            var resultValue = result as ObjectResult;
            Assert.Equal(422, resultValue.StatusCode);
        }

        [Fact]
        public void CreateLeague_ReturnsStatusCode500WhenTryToSave()
        {
            var createdLeagueDto = new LeagueDto() { Id = 3, LeagueName = "USA" };
            var createdLeague = new League() { Id = createdLeagueDto.Id, LeagueName = createdLeagueDto.LeagueName };
            _leagueRepositoryMock.Setup(mock => mock.GetLeagues())
                .Returns(GetLeagues());
            _leagueRepositoryMock.Setup(mock => mock.CreateLeague(createdLeague))
                .Returns(false);
            _mapper.Setup(m => m.Map<League>(createdLeagueDto))
                .Returns(createdLeague);
            _countryRepositoryMock.Setup(mock => mock.GetCountry(1))
                .Returns(It.IsAny<Country>);
            var controller = new LeagueController(
                           _leagueRepositoryMock.Object,
                           _teamRepositoryMock.Object,
                           _mapper.Object,
                           _countryRepositoryMock.Object);
            var result = controller.CreateLeague(1, createdLeagueDto) as ActionResult;

            var resultValue = result as ObjectResult;
            Assert.Equal(500, resultValue.StatusCode);
        }

        [Fact]
        public void CreateLeague_LeagueSuccessfullyCreated()
        {
            var createdLeagueDto = new LeagueDto() { Id = 3, LeagueName = "USA" };
            var createdLeague = new League() { Id = createdLeagueDto.Id, LeagueName = createdLeagueDto.LeagueName };
            _leagueRepositoryMock.Setup(mock => mock.GetLeagues())
                .Returns(GetLeagues());
            _leagueRepositoryMock.Setup(mock => mock.CreateLeague(createdLeague))
                .Returns(true);
            _countryRepositoryMock.Setup(mock => mock.GetCountry(1))
                .Returns(It.IsAny<Country>());
            _mapper.Setup(m => m.Map<League>(createdLeagueDto))
                .Returns(createdLeague);
            var controller = new LeagueController(
                           _leagueRepositoryMock.Object,
                           _teamRepositoryMock.Object,
                           _mapper.Object,
                           _countryRepositoryMock.Object);

            var result = controller.CreateLeague(1, createdLeagueDto) as ActionResult;

            var resultValue = result as ObjectResult;
            Assert.Equal(200, resultValue.StatusCode);
            Assert.Equal("Создано успешно", resultValue.Value);
        }

        [Fact]
        public void UpdateLeague_ReturnsBadRequestWhenUpdatedLeagueIsNull()
        {
            var updatedLeagueDto = new LeagueDto() { Id = 1, LeagueName = "RPL" };
            var updatedLeague = new League() { Id = updatedLeagueDto.Id, LeagueName = updatedLeagueDto.LeagueName };
            var controller = new LeagueController(
                          _leagueRepositoryMock.Object,
                          _teamRepositoryMock.Object,
                          _mapper.Object,
                          _countryRepositoryMock.Object);

            var result = controller.UpdateLeague(0, 0, null) as ActionResult;

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateLeague_ReturnsBadRequestWhenIdsNotMatch()
        {
            var updatedLeagueDto = new LeagueDto() { Id = 1, LeagueName = "RPL" };
            var updatedLeague = new League() { Id = updatedLeagueDto.Id, LeagueName = updatedLeagueDto.LeagueName };
            var controller = new LeagueController(
                         _leagueRepositoryMock.Object,
                         _teamRepositoryMock.Object,
                         _mapper.Object,
                         _countryRepositoryMock.Object);

            var result = controller.UpdateLeague(0, 1, updatedLeagueDto) as ActionResult;

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateLeague_ReturnsNotFound()
        {
            var updatedLeagueDto = new LeagueDto() { Id = 1, LeagueName = "RPL" };
            var updatedLeague = new League() { Id = updatedLeagueDto.Id, LeagueName = updatedLeagueDto.LeagueName };
            _leagueRepositoryMock.Setup(mock => mock.LeagueExists(updatedLeagueDto.Id))
                .Returns(false);
            var controller = new LeagueController(
                        _leagueRepositoryMock.Object,
                        _teamRepositoryMock.Object,
                        _mapper.Object,
                        _countryRepositoryMock.Object);

            var result = controller.UpdateLeague(1, 1, updatedLeagueDto) as ActionResult;

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void UpdateLeague_ReturnsStatusCode500WhenTryToSave()
        {
            var updatedLeagueDto = new LeagueDto() { Id = 1, LeagueName = "RPL" };
            var updatedLeague = new League() { Id = updatedLeagueDto.Id, LeagueName = updatedLeagueDto.LeagueName };
            _leagueRepositoryMock.Setup(mock => mock.LeagueExists(updatedLeagueDto.Id))
                .Returns(true);
            _leagueRepositoryMock.Setup(mock => mock.UpdateLeague(updatedLeague))
                .Returns(false);
            _mapper.Setup(m => m.Map<League>(updatedLeagueDto))
                .Returns(updatedLeague);
            var controller = new LeagueController(
                        _leagueRepositoryMock.Object,
                        _teamRepositoryMock.Object,
                        _mapper.Object,
                        _countryRepositoryMock.Object);

            var result = controller.UpdateLeague(1, 1, updatedLeagueDto) as ActionResult;

            var resultValue = result as ObjectResult;
            Assert.Equal(500, resultValue.StatusCode);
        }

        [Fact]
        public void UpdateLeague_ReturnsStatusCode200WithNoContent()
        {

            var updatedLeagueDto = new LeagueDto() { Id = 1, LeagueName = "RPL" };
            var updatedLeague = new League() { Id = updatedLeagueDto.Id, LeagueName = updatedLeagueDto.LeagueName };
            _leagueRepositoryMock.Setup(mock => mock.LeagueExists(updatedLeagueDto.Id))
                .Returns(true);
            _leagueRepositoryMock.Setup(mock => mock.UpdateLeague(updatedLeague))
                .Returns(true);
            _countryRepositoryMock.Setup(mock => mock.GetCountry(1))
                .Returns(It.IsAny<Country>);
            _mapper.Setup(m => m.Map<League>(updatedLeagueDto))
                .Returns(updatedLeague);
            var controller = new LeagueController(
                        _leagueRepositoryMock.Object,
                        _teamRepositoryMock.Object,
                        _mapper.Object,
                        _countryRepositoryMock.Object);

            var result = controller.UpdateLeague(1, 1, updatedLeagueDto) as ActionResult;
            
            Assert.IsType<NoContentResult>(result);
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
