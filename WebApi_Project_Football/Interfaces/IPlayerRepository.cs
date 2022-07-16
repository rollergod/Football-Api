using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi_Project_Football.Models;

namespace WebApi_Project_Football.Interfaces
{
    public interface IPlayerRepository
    {
        public bool PlayerExists(int id);
        public Player GetPlayerById(int id);
        public ICollection<Player> GetPlayers();
        public Statistic GetStatisticFromPlayer(int playerId);
        public Team GetTeamFromPlayer(int playerId);
    }
}
