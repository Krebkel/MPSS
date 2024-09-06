namespace Web.Controllers.Tokens;

public class UserCredentialsRequest
{
    public required string Phone { get; set; }
    
    public required string Password { get; set; }
}