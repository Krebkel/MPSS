using Contracts.ProjectEntities;
using Web.Requests.ProjectRequests;
using Web.Responses.ProjectResponses;

namespace Web.Extensions.ProjectExtensions;

public static class CounteragentExtension
{
    public static Counteragent ToCounteragent(this CreateCounteragentApiRequest apiRequest)
    {
        return new Counteragent
        {
            Name = apiRequest.Name,
            Contact = apiRequest.Contact,
            Phone = apiRequest.Phone,
            INN = apiRequest.INN,
            OGRN = apiRequest.OGRN,
            AccountNumber = apiRequest.AccountNumber,
            BIK = apiRequest.BIK
        };
    }

    public static Counteragent ToCounteragent(this UpdateCounteragentApiRequest apiRequest, int id)
    {
        return new Counteragent
        {
            Id = id,
            Name = apiRequest.Name,
            Contact = apiRequest.Contact,
            Phone = apiRequest.Phone,
            INN = apiRequest.INN,
            OGRN = apiRequest.OGRN,
            AccountNumber = apiRequest.AccountNumber,
            BIK = apiRequest.BIK
        };
    }

    public static ApiCounteragent ToApiCounteragent(this Counteragent counteragent)
    {
        return new ApiCounteragent
        {
            Id = counteragent.Id,
            Name = counteragent.Name,
            Contact = counteragent.Contact,
            Phone = counteragent.Phone,
            INN = counteragent.INN,
            OGRN = counteragent.OGRN,
            AccountNumber = counteragent.AccountNumber,
            BIK = counteragent.BIK
        };
    }
}