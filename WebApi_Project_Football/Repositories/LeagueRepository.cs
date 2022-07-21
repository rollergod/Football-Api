using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi_Project_Football.Data;
using WebApi_Project_Football.Interfaces;
using WebApi_Project_Football.Models;

namespace WebApi_Project_Football.Repositories
{
    public class LeagueRepository : ILeagueRepository
    {
        private readonly AppDbContext _context;

        public LeagueRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool CreateLeague(League league)
        {
            _context.Add(league);
            return SaveLeague();
        }

        public bool DeleteLeague(League league)
        {
            _context.Remove(league);
            return SaveLeague();
        }

        public League GetLeague(int id)
        {
            return _context.Leagues.FirstOrDefault(x => x.Id == id);
        }

        public League GetLeagueFromTeam(int teamId)
        {
            return _context.Teams.Where(t => t.Id == teamId).Select(t => t.League).FirstOrDefault();
        }

        public ICollection<League> GetLeagues()
        {
            return _context.Leagues.ToList();
        }

        public ICollection<Team> GetTeamsFromLeague(int leagueId)
        {
            return _context.Teams.Where(t => t.LeagueId == leagueId).ToList();
        }

        public bool LeagueExists(int id)
        {
            return _context.Leagues.Any(l => l.Id == id);
        }

        public bool SaveLeague()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateLeague(League league)
        {
            _context.Update(league);
            return SaveLeague();
        }
    }
}
