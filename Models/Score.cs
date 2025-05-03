using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaderboardService.Models
{
    public class Score
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Game {get; set; } = string.Empty;
        public int Points { get; set; }
        public DateTime Date { get; set; } 
    }
}