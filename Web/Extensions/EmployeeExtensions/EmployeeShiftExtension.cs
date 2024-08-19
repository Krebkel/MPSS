using Contracts.EmployeeEntities;
using Web.Requests.EmployeeRequests;
using Web.Responses.EmployeeResponses;

namespace Web.Extensions.EmployeeExtensions;

public static class EmployeeShiftExtension
{
    public static EmployeeShift ToEmployeeShift(this CreateEmployeeShiftApiRequest apiRequest)
    {
        return new EmployeeShift
        {
            ProjectId = apiRequest.ProjectId,
            EmployeeId = apiRequest.EmployeeId,
            Date = apiRequest.Date,
            Arrival = apiRequest.Arrival,
            Departure = apiRequest.Departure,
            HoursWorked = apiRequest.HoursWorked,
            TravelTime = apiRequest.TravelTime,
            ConsiderTravel = apiRequest.ConsiderTravel
        };
    }

    public static EmployeeShift ToEmployeeShift(this UpdateEmployeeShiftApiRequest apiRequest, int id)
    {
        return new EmployeeShift
        {
            Id = id,
            ProjectId = apiRequest.ProjectId,
            EmployeeId = apiRequest.EmployeeId,
            Date = apiRequest.Date,
            Arrival = apiRequest.Arrival,
            Departure = apiRequest.Departure,
            HoursWorked = apiRequest.HoursWorked,
            TravelTime = apiRequest.TravelTime,
            ConsiderTravel = apiRequest.ConsiderTravel
        };
    }

    public static ApiEmployeeShift ToApiEmployeeShift(this EmployeeShift employeeShift)
    {
        return new ApiEmployeeShift
        {
            Id = employeeShift.Id,
            ProjectId = employeeShift.ProjectId,
            EmployeeId = employeeShift.EmployeeId,
            Date = employeeShift.Date,
            Arrival = employeeShift.Arrival,
            Departure = employeeShift.Departure,
            HoursWorked = employeeShift.HoursWorked,
            TravelTime = employeeShift.TravelTime,
            ConsiderTravel = employeeShift.ConsiderTravel
        };
    }
}