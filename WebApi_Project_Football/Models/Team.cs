using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi_Project_Football.Models
{
    public class Team
    {
        public int Id { get; set; }
        public int LeagueId { get; set; }
        public League League { get; set; }
        public string TeamName { get; set; }
        public List<Player> Players { get; set; }
    }
}
