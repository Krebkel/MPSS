using Contracts.EmployeeEntities;
using Web.Requests.EmployeeRequests;
using Web.Responses.EmployeeResponses;

namespace Web.Extensions.EmployeeExtensions;

public static class EmployeeExtension
{
    public static Employee ToEmployee(this CreateEmployeeApiRequest apiRequest)
    {
        return new Employee
        {
            Name = apiRequest.Name,
            Phone = apiRequest.Phone,
            IsDriver = apiRequest.IsDriver,
            Passport = apiRequest.Passport,
            DateOfBirth = apiRequest.DateOfBirth,
            INN = apiRequest.INN,
            AccountNumber = apiRequest.AccountNumber,
            BIK = apiRequest.BIK
        };
    }

    public static Employee ToEmployee(this UpdateEmployeeApiRequest apiRequest, int id)
    {
        return new Employee
        {
            Id = id,
            Name = apiRequest.Name,
            Phone = apiRequest.Phone,
            IsDriver = apiRequest.IsDriver,
            Passport = apiRequest.Passport,
            DateOfBirth = apiRequest.DateOfBirth,
            INN = apiRequest.INN,
            AccountNumber = apiRequest.AccountNumber,
            BIK = apiRequest.BIK
        };
    }

    public static ApiEmployee ToApiEmployee(this Employee employee)
    {
        return new ApiEmployee
        {
            Id = employee.Id,
            Name = employee.Name,
            Phone = employee.Phone,
            IsDriver = employee.IsDriver,
            Passport = employee.Passport,
            DateOfBirth = employee.DateOfBirth,
            INN = employee.INN,
            AccountNumber = employee.AccountNumber,
            BIK = employee.BIK
        };
    }
}