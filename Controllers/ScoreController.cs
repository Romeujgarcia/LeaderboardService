using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace LeaderboardService;

[ApiController]
[Route("api/[controller]")]
public class ScoreController : ControllerBase
{
    private readonly ScoreService _scoreService;

    public ScoreController(ScoreService scoreService)
    {
        _scoreService = scoreService;
    }

    [HttpPost("submit")]
    public async Task<IActionResult> SubmitScore([FromBody] SubmitScoreDto scoreDto)
    {
        await _scoreService.SubmitScore(scoreDto.UserId, scoreDto.Game, scoreDto.Points);
        return Ok();
    }
}