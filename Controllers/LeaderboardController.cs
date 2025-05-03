using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LeaderboardService.Services; // Corrigido

[ApiController]
[Route("api/[controller]")]
public class LeaderboardController : ControllerBase
{
    private readonly LeaderboardServiceHandler _leaderboardService; // Certifique-se de que o nome da variável está correto

    public LeaderboardController(LeaderboardServiceHandler leaderboardService) // Certifique-se de que o nome do parâmetro está correto
    {
        _leaderboardService = leaderboardService; // Certifique-se de que o nome da variável está correto
    }

    [HttpGet("{game}")]
    public async Task<IActionResult> GetLeaderboard(string game)
    {
        var leaderboard = await _leaderboardService.GetLeaderboard(game); // Certifique-se de que o nome da variável está correto
        return Ok(leaderboard);
    }
}