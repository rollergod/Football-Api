using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi_Project_Football.Dto;
using WebApi_Project_Football.Models;

namespace WebApi_Project_Football.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Player, PlayerDto>();
            CreateMap<League, LeagueDto>();
            CreateMap<Statistic, StatisticDto>();
            CreateMap<Team, TeamDto>();
            CreateMap<Country, CountryDto>();

            CreateMap<PlayerDto, Player>();
            CreateMap<LeagueDto, League>();
            CreateMap<StatisticDto, Statistic>();
            CreateMap<TeamDto, Team>();
            CreateMap<CountryDto, Country>();
        }
    }
}
