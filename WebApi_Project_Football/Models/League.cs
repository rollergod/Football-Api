using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi_Project_Football.Models
{
    public class League
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public string LeagueName { get; set; }
        public List<Team> Teams { get; set; }
    }
}
