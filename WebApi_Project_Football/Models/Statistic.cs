using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi_Project_Football.Models
{
    public class Statistic
    {
        public int Id { get; set; }
        public int GamesPlayed { get; set; }
        public int MinutesPlayed { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
        public int? CleanSheets { get; set; }

        public Player Player { get; set; }
    }
}
