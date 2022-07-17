using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi_Project_Football.Models
{
    public class Player
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
        public int CountryId { get; set; }
        public Country Country{ get; set; }
        public int StatisticId { get; set; }
        //[NotMapped]
        public Statistic Statistic { get; set; }
        //public List<Statistic> Statistics { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public int Age { get; set; }
    }

}
