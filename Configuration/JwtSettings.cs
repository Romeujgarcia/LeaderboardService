namespace LeaderboardService.Configuration
{
    public class JwtSettings
    {
        public string SecretKey { get; set; }      // Use SecretKey ou Secret, mas mantenha consistência
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryMinutes { get; set; }
    }
}