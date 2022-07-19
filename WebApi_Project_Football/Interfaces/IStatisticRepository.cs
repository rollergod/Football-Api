using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi_Project_Football.Models;

namespace WebApi_Project_Football.Interfaces
{
    public interface IStatisticRepository
    {
        public bool StatisticExists(int id);
        public ICollection<Statistic> GetStatistics();
        public Statistic GetStatistic(int id);
        public ICollection<Player> GetPlayersFromStatistic(int statId);
        public bool CreateStatistic(Statistic statistic);
        public bool UpdateStatistic(Statistic statistic);
        public bool SaveStatistic();
        
    }
}
