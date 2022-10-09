using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi_Project_Football.Controllers;
using WebApi_Project_Football.Dto;
using WebApi_Project_Football.Interfaces;
using WebApi_Project_Football.Models;
using Xunit;

namespace WebApi_Project_Football.Tests
{
    public class PlayerRepositoryTests
    {
        private readonly Mock<IMapper> _mapper = new Mock<IMapper>();
        private readonly Mock<IPlayerRepository> _playerRepositoryMock = new Mock<IPlayerRepository>();
        private readonly Mock<ITeamRepository> _teamRepositoryMock = new Mock<ITeamRepository>();
        private readonly Mock<IStatisticRepository> _statisticRepositoryMock = new Mock<IStatisticRepository>();
        private readonly Mock<ICountryRepository> _countryRepositoryMock = new Mock<ICountryRepository>();


        [Fact]
        public void GetPlayers_ReturnsPlayers()
        {
            _playerRepositoryMock.Setup(repo => repo.GetPlayers()).Returns(GetPlayers());
            _mapper.Setup(m => m.Map<List<PlayerDto>>(It.IsAny<List<Player>>()))
                .Returns(GetPlayersDto());

            var controller = new PlayerController(
                                    _playerRepositoryMock.Object,
                                    _mapper.Object,
                                    _teamRepositoryMock.Object,
                                    _statisticRepositoryMock.Object,
                                    _countryRepositoryMock.Object);

            var result = controller.GetPlayers() as OkObjectResult;

            var resultList = result.Value as List<PlayerDto>;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Renat", resultList[0].FirstName);
        }

        [Fact]
        public void GetPlayerById_ReturnsBadRequest()
        {
            int testId = -1;
            _playerRepositoryMock.Setup(repo => repo.GetPlayerById(testId));

            var controller = new PlayerController(
                                  _playerRepositoryMock.Object,
                                  _mapper.Object,
                                  _teamRepositoryMock.Object,
                                  _statisticRepositoryMock.Object,
                                  _countryRepositoryMock.Object);

            var result = controller.GetPlayerById(testId) as BadRequestResult;

            Assert.IsType<BadRequestResult>(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void GetPlayerById_ReturnsNotFound()
        {
            int testId = 2;
            _playerRepositoryMock.Setup(repo => repo.PlayerExists(testId))
                .Returns(GetPlayers().Any(player => player.Id == testId));

            var controller = new PlayerController(
                                _playerRepositoryMock.Object,
                                _mapper.Object,
                                _teamRepositoryMock.Object,
                                _statisticRepositoryMock.Object,
                                _countryRepositoryMock.Object);

            var result = controller.GetPlayerById(testId) as NotFoundResult;

            Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, result.StatusCode);
        }

        public List<Player> GetPlayers()
        {
            return new List<Player>
            {
                new Player {Id = 1, FirstName = "Renat",LastName = "Apaev"},
            };
        }

        public List<PlayerDto> GetPlayersDto()
        {
            return new List<PlayerDto>
            {
                new PlayerDto {Id = 1, FirstName = "Renat",LastName = "Apaev"},
            };
        }
    }
}
