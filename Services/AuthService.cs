using StackExchange.Redis;
using BCrypt.Net; // Adicione esta linha para usar o BCrypt
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LeaderboardService.Models; // Para usar a classe User

public class AuthService
{
    private readonly IDatabase _redisDb;
    private static readonly byte[] _jwtSecretKey;

    // Inicializador estático para gerar a chave apenas uma vez
    static AuthService()
    {
        // Gerar uma chave secreta de 32 bytes (256 bits)
        _jwtSecretKey = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(_jwtSecretKey);
        }
    }

    public AuthService(IConnectionMultiplexer redis)
    {
        _redisDb = redis.GetDatabase();
    }

    public async Task<Usuario> Register(string username, string password)
    {
        if (await _redisDb.StringGetAsync($"user:{username}") != RedisValue.Null)
        {
            throw new Exception("User already exists");
        }
        //Console.WriteLine($"Senha que está sendo registrada: {password}");
        var passwordHash = HashPassword(password);
        var user = new Usuario { Username = username, PasswordHash = passwordHash };

        // Armazenar o hash da senha no Redis
        await _redisDb.StringSetAsync($"user:{username}", passwordHash);
        return user;
    }

    public async Task<string> Login(string username, string password)
    {
        //Console.WriteLine($"Senha que está sendo usada para login: {password}");
        // Recupera o hash armazenado no Redis para o usuário fornecido
        var storedHash = await _redisDb.StringGetAsync($"user:{username}");

        // Verifica se o hash armazenado é nulo
        if (storedHash.IsNull)
        {
            Console.WriteLine($"No stored hash found for user: {username}");
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        // Verifica se a senha fornecida corresponde ao hash armazenado
        if (!VerifyPassword(password, storedHash))
        {
            Console.WriteLine($"Invalid password for user: {username}");
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        // Gerar e retornar um token JWT
        return GenerateJwtToken(username);
    }

    private string HashPassword(string password)
    {
        // Usando BCrypt para gerar o hash da senha
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string password, string storedHash)
    {
        // Verifica se a senha fornecida corresponde ao hash armazenado
        return BCrypt.Net.BCrypt.Verify(password, storedHash);
    }

    private string GenerateJwtToken(string username)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(_jwtSecretKey); // Use a chave secreta gerada

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
            Expires = DateTime.UtcNow.AddHours(1), // Define a expiração do token
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}