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
    public class TeamRepository : ITeamRepository
    {
        private readonly AppDbContext _context;
        public TeamRepository(AppDbContext context)
        {
            _context = context;
        }
        public League GetLeagueFromTeam(int teamId)
        {
            return _context.Teams.Where(t => t.Id == teamId).Select(t => t.League).FirstOrDefault();
        }

        public ICollection<Player> GetPlayersFromTeam(int teamId)
        {
            return _context.Players.Where(p => p.TeamId == teamId).ToList();
        }

        public Team GetTeamById(int id)
        {
            return _context.Teams.FirstOrDefault(t => t.Id == id);
        }

        public ICollection<Team> GetTeams()
        {
            return _context.Teams.ToList();
        }

        public bool TeamExists(int id)
        {
            return _context.Teams.Any(t => t.Id == id);
        }
    }
}
