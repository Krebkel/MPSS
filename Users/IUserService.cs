using Contracts;
using Users.Requests;

namespace Users;

public interface IUserService
{
    Task<User?> FindUser(string phoneNumber, CancellationToken cancellationToken);
    
    Task Register(UserRegistrationRequest request, CancellationToken cancellationToken);

}