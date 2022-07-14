using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi_Project_Football.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string CountryName { get; set; }
        public List<League> Leagues { get; set; }
    }
}
