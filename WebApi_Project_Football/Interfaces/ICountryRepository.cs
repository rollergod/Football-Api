using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi_Project_Football.Models;

namespace WebApi_Project_Football.Interfaces
{
    public interface ICountryRepository
    {
        bool CountryExist(int id);
        public Country GetCountry(int id);
        ICollection<Country> GetCountries();
        ICollection<League> GetLeaguesFromCountry(int countryId);
    }
}
