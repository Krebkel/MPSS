using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Users;
using Web.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Web.Controllers.Tokens;

[Route("api/token")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IUserService _users;
    private readonly JwtTokenOptions _options;

    public TokenController(IOptions<JwtTokenOptions> options, IUserService users)
    {
        _users = users;
        _options = options.Value;
    }

    [HttpPost]
    public async Task<IActionResult> Post(UserCredentialsRequest request, CancellationToken ct)
    {
        var user = await _users.FindUser(request.Phone, ct);
        if (user == null) return BadRequest("Пользователь не найден");
        if (user.Password != request.Password) return BadRequest("Неправильный пароль");
        
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Employee.Phone),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
            new Claim("DisplayName", user.Employee.Name),
            new Claim("Phone", user.Employee.Phone),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        var tokenHandler = new JwtSecurityTokenHandler();
        var encodedToken = tokenHandler.WriteToken(token);
        
        return Ok(new { token = encodedToken });
    }
}