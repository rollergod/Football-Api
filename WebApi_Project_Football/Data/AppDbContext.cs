using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi_Project_Football.Models;

namespace WebApi_Project_Football.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
        {
            
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Statistic> Statistics { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //просто так... для примера
            modelBuilder.ApplyConfiguration(new LeagueConfiguration());
            modelBuilder.ApplyConfiguration(new PlayerConfiguration());
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new TeamConfiguration());

            modelBuilder
                .Entity<Country>()
                .HasData(
                    new Country { Id = 1, CountryName = "Russia" },
                    new Country { Id = 2, CountryName = "England" },
                    new Country { Id = 3, CountryName = "France" }
                );

            modelBuilder
                .Entity<League>()
                .HasData(
                    new League { Id = 1, CountryId = 1, LeagueName = "Russian Premier League" },
                    new League { Id = 2, CountryId = 1, LeagueName = "FNL" },
                    new League { Id = 3, CountryId = 2, LeagueName = "England Premier League" },
                    new League { Id = 4, CountryId = 2, LeagueName = "English Football League Championship" },
                    new League { Id = 5, CountryId = 3, LeagueName = "League One" }
                );

            modelBuilder
                .Entity<Statistic>()
                .HasData(
                    new Statistic { Id = 1, GamesPlayed = 30, MinutesPlayed = 2315, Goals = 5, Assists = 5, RedCards = 1, YellowCards = 5 },
                    new Statistic { Id = 2, GamesPlayed = 12, MinutesPlayed = 765, Goals = 9, Assists = 0, RedCards = 0, YellowCards = 3 },
                    new Statistic { Id = 3, GamesPlayed = 21, MinutesPlayed = 1540, Goals = 1, Assists = 1, RedCards = 0, YellowCards = 1 }
                );

            modelBuilder
                .Entity<Team>()
                .HasData(
                    new Team { Id = 1, LeagueId = 1, TeamName = "Spartak Moscow" },
                    new Team { Id = 2, LeagueId = 1, TeamName = "Dinamo Moscow" },
                    new Team { Id = 3, LeagueId = 2, TeamName = "Rubin Kazan" },
                    new Team { Id = 4, LeagueId = 3, TeamName = "Arsenal" },
                    new Team { Id = 5, LeagueId = 4, TeamName = "Watford" },
                    new Team { Id = 6, LeagueId = 5, TeamName = "Monaco" }
                );

            modelBuilder
                .Entity<Player>()
                .HasData(
                    new Player { Id = 1, Position = "Defender", FirstName = "Georgiy", LastName = "Djikia", TeamId = 1, Age = 27, CountryId = 1, StatisticId = 3 },
                    new Player { Id = 2, Position = "Midfielder", FirstName = "Alexander", LastName = "Golovin", TeamId = 6, Age = 26, CountryId = 1, StatisticId = 1 },
                    new Player { Id = 3, Position = "Forward", FirstName = "Eddie", LastName = "Nketiah", TeamId = 4, Age = 23, CountryId = 2 ,StatisticId = 2}
                );
        }
    }
}
