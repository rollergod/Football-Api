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
    public class CountryRepository : ICountryRepository
    {
        private readonly AppDbContext _context;
        public CountryRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool CountryExist(int id)
        {
            return _context.Countries.Any(c => c.Id == id);
        }

        public ICollection<Country> GetCountries()
        {
            return _context.Countries.ToList();
        }

        public Country GetCountry(int id)
        {
            return _context.Countries.FirstOrDefault(c => c.Id == id);
        }

        public ICollection<League> GetLeaguesFromCountry(int countryId)
        {
            return _context.Leagues.Where(c => c.CountryId == countryId).ToList();
        }
    }
}
