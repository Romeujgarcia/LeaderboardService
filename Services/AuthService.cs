using StackExchange.Redis;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LeaderboardService.Models; // Para usar a classe User


public class AuthService
{
     private readonly IDatabase _redisDb;

    public AuthService(IConnectionMultiplexer redis)
    {
        _redisDb = redis.GetDatabase();
    }

    public async Task<Usuario> Register(string username, string password)
    {
        var passwordHash = HashPassword(password);
        var user = new Usuario { Username = username, PasswordHash = passwordHash };
        
        // Armazenar o usuário no Redis (ou em um banco de dados, se preferir)
        await _redisDb.StringSetAsync($"user:{username}", passwordHash);
        return user;
    }

    public async Task<string> Login(string username, string password)
    {
        var storedHash = await _redisDb.StringGetAsync($"user:{username}");
        if (storedHash.IsNull || storedHash != HashPassword(password))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        // Gerar e retornar um token JWT
        return GenerateJwtToken(username);
    }

    private string HashPassword(string password)
    {
        using var hmac = new HMACSHA256();
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hash);
    }

   private string GenerateJwtToken(string username)
{
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.UTF8.GetBytes("your_secret_key"); // Substitua pela sua chave secreta

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
        Expires = DateTime.UtcNow.AddHours(1), // Define a expiração do token
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    var tokenString = tokenHandler.WriteToken(token);   
    return tokenString; // Retornar o token gerado
}
}
