namespace Web.Options;

public class JwtTokenOptions
{
    public required string Subject { get; init; }
    
    public required string Key { get; init; }
    
    public required string Issuer { get; init; }
    
    public required string Audience { get; init; }
}