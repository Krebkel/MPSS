using System.ComponentModel.DataAnnotations;

namespace Users.Requests;

public class UserRegistrationRequest
{
    public required string Name { get; set; }
    public required string PhoneNumber { get; set; }
    
    [MinLength(8)]
    public required string Password { get; set; }
}