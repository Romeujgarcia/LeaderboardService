using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaderboardService.Services
{
    public class LeaderboardServiceHandler
    {
        private readonly IDatabase _redisDb;

        public LeaderboardServiceHandler(IConnectionMultiplexer redis)
        {
            _redisDb = redis.GetDatabase();
        }

        public async Task<List<(string UserId, double Score)>> GetLeaderboard(string game, int top = 10)
        {
            var key = $"leaderboard:{game}";
            var entries = await _redisDb.SortedSetRangeByRankWithScoresAsync(key, 0, top - 1, Order.Descending);
            return entries.Select(e => (UserId: e.Element.ToString(), Score: e.Score)).ToList();
        }
    }
}