namespace LeaderboardService;

public class SubmitScoreDto
{
    public int UserId { get; set; }
    public string Game { get; set; }
    public int Points { get; set; }
}
