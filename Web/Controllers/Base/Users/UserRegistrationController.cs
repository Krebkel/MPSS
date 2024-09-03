using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Users;
using Users.Requests;

namespace Web.Controllers.Base.Users;

[ApiController]
[Route("api/users")]
public class UserRegistrationController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserRegistrationController> _logger;

    public UserRegistrationController(IUserService userRegistrationService, ILogger<UserRegistrationController> logger)
    {
        _userService = userRegistrationService;
        _logger = logger;
    }

    [HttpPost("registration")]
    public async Task<IActionResult> RegisterUser(UserRegistrationRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _userService.Register(request, cancellationToken);
            return Ok();
        }
        catch (InvalidOperationException e)
        {
            _logger.LogError(e, "Ошибка при регистрации пользователя");
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка при регистрации пользователя");
            return BadRequest("Ошибка при регистрации пользователя");
        }
    }
}