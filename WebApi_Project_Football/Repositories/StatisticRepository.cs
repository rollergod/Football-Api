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
    public class StatisticRepository : IStatisticRepository
    {
        private readonly AppDbContext _context;

        public StatisticRepository(AppDbContext context)
        {
            _context = context;
        }

        public Player GetPlayerFromStatistic(int statId)
        {
            return _context.Statistics.Where(s => s.Id == statId).Select(s => s.Player).FirstOrDefault();
        }

        public Statistic GetStatistic(int id)
        {
            return _context.Statistics.FirstOrDefault(s => s.Id == id);
        }

        public ICollection<Statistic> GetStatistics()
        {
            return _context.Statistics.ToList();
        }

        public bool StatisticExists(int id)
        {
            return _context.Statistics.Any(c => c.Id == id);
        }
    }
}
