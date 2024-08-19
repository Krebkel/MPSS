using Contracts.ProjectEntities;
using Web.Requests.ProjectRequests;
using Web.Responses.ProjectResponses;

namespace Web.Extensions.ProjectExtensions;

public static class ExpenseExtensions
{
    public static Expense ToExpense(this CreateExpenseApiRequest apiRequest)
    {
        return new Expense
        {
            ProjectId = apiRequest.ProjectId,
            Name = apiRequest.Name,
            Amount = apiRequest.Amount,
            Description = apiRequest.Description,
            Type = apiRequest.Type,
            IsPaidByCompany = apiRequest.IsPaidByCompany
        };
    }

    public static Expense ToExpense(this UpdateExpenseApiRequest apiRequest, int id)
    {
        return new Expense
        {
            Id = id,
            ProjectId = apiRequest.ProjectId,
            Name = apiRequest.Name,
            Amount = apiRequest.Amount,
            Description = apiRequest.Description,
            Type = apiRequest.Type,
            IsPaidByCompany = apiRequest.IsPaidByCompany
        };
    }

    public static ApiExpense ToApiExpense(this Expense expense)
    {
        return new ApiExpense
        {
            Id = expense.Id,
            ProjectId = expense.ProjectId,
            Name = expense.Name,
            Amount = expense.Amount,
            Description = expense.Description,
            Type = expense.Type,
            IsPaidByCompany = expense.IsPaidByCompany
        };
    }
}