using Contracts;
using DataContracts;
using Employees.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Users.Requests;

namespace Users;

internal class UserService : IUserService
{

    private readonly IRepository<User> _userRepository;
    private readonly ILogger<UserService> _logger;
    private readonly IEmployeeService _employeeService; 


    public UserService(
        IRepository<User> userRepository,
        ILogger<UserService> logger,
        IEmployeeService employeeService)
    {
        _userRepository = userRepository;
        _logger = logger;
        _employeeService = employeeService; 
    }

    public async Task<User?> FindUser(string phoneNumber, CancellationToken cancellationToken)
    {
        return await _userRepository.GetAll()
            .Include(u => u.Employee)
            .Where(u => u.Employee.Phone == phoneNumber)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task Register(UserRegistrationRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var createdEmployee = await _employeeService.CreateEmployeeAsync(new CreateEmployeeRequest
            {
                Name = request.Name,
                Phone = request.PhoneNumber
            }, cancellationToken);

            var user = new User
            {
                Employee = createdEmployee,
                Password = request.Password,
                Role = UserRole.Regular
            };

            await _userRepository.AddAsync(user, cancellationToken);

            _logger.LogInformation("Новый пользователь успешно зарегистрирован: {@User}", user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при регистрации пользователя");
            throw;
        }
    }
}