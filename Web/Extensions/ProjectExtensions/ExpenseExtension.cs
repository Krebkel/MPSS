using Projects.Services;
using Web.Requests.ProjectRequests;

namespace Web.Extensions.ProjectExtensions;

public static class ExpenseExtensions
{
    internal static CreateExpenseRequest ToCreateExpenseApiRequest(this CreateExpenseApiRequest request)
    {
        return new CreateExpenseRequest
        {
            Project = request.Project,
            Name = request.Name,
            Amount = request.Amount,
            Description = request.Description,
            Type = request.Type,
            IsPaidByCompany = request.IsPaidByCompany
        };
    }
    
    internal static UpdateExpenseRequest ToUpdateExpenseApiRequest(this UpdateExpenseApiRequest request)
    {
        return new UpdateExpenseRequest
        {
            Id = request.Id,
            Project = request.Project,
            Name = request.Name,
            Amount = request.Amount,
            Description = request.Description,
            Type = request.Type,
            IsPaidByCompany = request.IsPaidByCompany
        };
    }
}