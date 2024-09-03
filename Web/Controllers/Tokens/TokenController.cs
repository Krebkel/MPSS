using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Users;
using Web.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

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
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, _options.Subject),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
            new Claim("DisplayName", $"{user.Employee.Name}"),
            new Claim("Phone", user.Employee.Phone)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: signIn);

        return Ok(new JwtSecurityTokenHandler().WriteToken(token));
    }
}