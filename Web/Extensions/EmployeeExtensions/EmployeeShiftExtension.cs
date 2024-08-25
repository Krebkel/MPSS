using Employees.Services;
using Web.Requests.EmployeeRequests;

namespace Web.Extensions.EmployeeExtensions;

public static class EmployeeShiftExtension
{
    internal static CreateEmployeeShiftRequest ToCreateEmployeeShiftApiRequest(this CreateEmployeeShiftApiRequest request)
    {
        return new CreateEmployeeShiftRequest
        {
            Project = request.Project,
            Employee = request.Employee,
            Date = request.Date,
            Arrival = request.Arrival,
            Departure = request.Departure,
            HoursWorked = request.HoursWorked,
            TravelTime = request.TravelTime,
            ConsiderTravel = request.ConsiderTravel,
            ISN = request.ISN
        };
    }
    
    internal static UpdateEmployeeShiftRequest ToUpdateEmployeeShiftApiRequest(this UpdateEmployeeShiftApiRequest request)
    {
        return new UpdateEmployeeShiftRequest
        {
            Id = request.Id,
            Project = request.Project,
            Employee = request.Employee,
            Date = request.Date,
            Arrival = request.Arrival,
            Departure = request.Departure,
            HoursWorked = request.HoursWorked,
            TravelTime = request.TravelTime,
            ConsiderTravel = request.ConsiderTravel,
            ISN = request.ISN
        };
    }
}