using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaderboardService.Models
{
    public class LeaderboardEntry
    {
        public string UserName { get; set; }

        public int TotalPoints { get; set; }
    }
}