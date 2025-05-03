using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace LeaderboardService;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var user = await _authService.Register(registerDto.Username, registerDto.Password);
        return CreatedAtAction(nameof(Login), new { id = user.Id }, user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var token = await _authService.Login(loginDto.Username, loginDto.Password);
        return Ok(new { Token = token });
    }
}
