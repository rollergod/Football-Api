﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi_Project_Football.Data;
using WebApi_Project_Football.Interfaces;
using WebApi_Project_Football.Models;

namespace WebApi_Project_Football.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly AppDbContext _context;

        public PlayerRepository(AppDbContext context)
        {
            _context = context;
        }

        public Player GetPlayerById(int id)
        {
            return _context.Players.FirstOrDefault(p => p.Id == id);
        }

        public ICollection<Player> GetPlayers()
        {
            return _context.Players.ToList();
        }

        public Statistic GetStatisticFromPlayer(int playerId)
        {
            //return _context.Players.FirstOrDefault(p => p.Id == playerId).Statistic; не будет работать, ибо statistic == null
            return _context.Players.Where(p => p.Id == playerId).Select(p => p.Statistic).FirstOrDefault();
        }

        public Team GetTeamFromPlayer(int playerId)
        {
            return _context.Players.Where(p => p.Id == playerId).Select(p => p.Team).FirstOrDefault();
        }

        public bool PlayerExists(int id)
        {
            return _context.Players.Any(p => p.Id == id);
        }
    }
}