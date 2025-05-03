using LeaderboardService.Models; // Adicione esta linha no início do arquivo

namespace LeaderboardService.Models
{
    public class Usuario
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
