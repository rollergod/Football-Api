﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi_Project_Football.Models;

namespace WebApi_Project_Football.Interfaces
{
    public interface ITeamRepository
    {
        public bool TeamExists(int id);
        public Team GetTeamById(int id);
        public ICollection<Team> GetTeams();
        public ICollection<Player> GetPlayersFromTeam(int teamId);
        public League GetLeagueFromTeam(int teamId);
    }
}