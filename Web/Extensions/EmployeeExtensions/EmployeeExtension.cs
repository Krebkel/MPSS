using Employees.Services;
using Web.Requests.EmployeeRequests;

namespace Web.Extensions.EmployeeExtensions;

public static class EmployeeExtension
{
    internal static CreateEmployeeRequest ToCreateEmployeeRequest(this CreateEmployeeApiRequest request)
    {
        return new CreateEmployeeRequest
        {
            Name = request.Name,
            Phone = request.Phone,
            IsDriver = request.IsDriver,
            Passport = request.Passport,
            DateOfBirth = request.DateOfBirth,
            INN = request.INN,
            AccountNumber = request.AccountNumber,
            BIK = request.BIK
        };
    }
    
    internal static UpdateEmployeeRequest ToUpdateEmployeeRequest(this UpdateEmployeeApiRequest request)
    {
        return new UpdateEmployeeRequest
        {
            Id = request.Id,
            Name = request.Name,
            Phone = request.Phone,
            IsDriver = request.IsDriver,
            Passport = request.Passport,
            DateOfBirth = request.DateOfBirth,
            INN = request.INN,
            AccountNumber = request.AccountNumber,
            BIK = request.BIK
        };
    }
}