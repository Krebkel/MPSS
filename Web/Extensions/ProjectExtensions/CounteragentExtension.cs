using Projects.Services;
using Web.Requests.ProjectRequests;

namespace Web.Extensions.ProjectExtensions;

public static class CounteragentExtension
{
    internal static CreateCounteragentRequest ToCreateCounteragentApiRequest(this CreateCounteragentApiRequest request)
    {
        return new CreateCounteragentRequest
        {
            Name = request.Name,
            Contact = request.Contact,
            Phone = request.Phone,
            INN = request.INN,
            OGRN = request.OGRN,
            AccountNumber = request.AccountNumber,
            BIK = request.BIK
        };
    }
    
    internal static UpdateCounteragentRequest ToUpdateCounteragentApiRequest(this UpdateCounteragentApiRequest request)
    {
        return new UpdateCounteragentRequest
        {
            Id = request.Id,
            Name = request.Name,
            Contact = request.Contact,
            Phone = request.Phone,
            INN = request.INN,
            OGRN = request.OGRN,
            AccountNumber = request.AccountNumber,
            BIK = request.BIK
        };;
    }
}