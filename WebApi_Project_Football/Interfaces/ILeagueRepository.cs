using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi_Project_Football.Models;

namespace WebApi_Project_Football.Interfaces
{
    public interface ILeagueRepository
    {
        public bool LeagueExists(int id);
        public League GetLeague(int id);
        public ICollection<League> GetLeagues();
        public ICollection<Team> GetTeamsFromLeague(int leagueId);
        public League GetLeagueFromTeam(int teamId);
        public bool CreateLeague(League league);
        bool UpdateLeague(League league);
        public bool SaveLeague();
    }
}
