﻿using GotExplorer.DAL.Entities;
using GotExplorer.DAL.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace GotExplorer.DAL.Entities
{
    public class Game : IEntity
    {
        public int Id { get; set; }

        public int Score { get; set; }

        public TimeSpan SpentTime { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public GameType GameType { get; set; }

        public List<Level> Levels { get; set; } = new List<Level>();
    }
}
