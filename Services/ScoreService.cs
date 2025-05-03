using StackExchange.Redis;


public class ScoreService
{
   private readonly IDatabase _redisDb;

    public ScoreService(IConnectionMultiplexer redis)
    {
        _redisDb = redis.GetDatabase();
    }

     public async Task SubmitScore(int userId, string game, int points)
    {
        // Usa Sorted Set com chave por jogo para guardar pontuação do usuário
        var key = $"leaderboard:{game}";
        await _redisDb.SortedSetAddAsync(key, userId.ToString(), points);
    }
}
