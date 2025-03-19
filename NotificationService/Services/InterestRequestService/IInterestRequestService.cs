using NotificationService.Dtos;
using NotificationService.Models;

namespace NotificationService.Services.IntrestRequestService
{
    public interface IInterestRequestService
    {
        Task<string> AddInterestRequest(InterestRequestDto intrestRequestDto);

        Task<string> WithdrawInterstRequest(Guid id);
        Task<string> RejectInterestRequest(Guid id);

        Task<string> AcceptInterestRequest(AcceptInterestDto acceptInterestDto);

        Task<List<InterestRequest>> GetInterestRequestForEmployers(Guid eId);
        Task<List<InterestRequest>> GetAcceptedInterestRequestOfLabour(Guid Lid);

    }
}
