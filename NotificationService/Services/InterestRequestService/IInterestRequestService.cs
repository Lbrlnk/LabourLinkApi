using NotificationService.Dtos;

namespace NotificationService.Services.IntrestRequestService
{
    public interface IInterestRequestService
    {
        Task<string> AddInterestRequest(InterestRequestDto intrestRequestDto);

        Task<string> WithdrawInterstRequest(Guid id);
        Task<string> RejectInterestRequest(Guid id);

        Task<string> AcceptInterestRequest(AcceptInterestDto acceptInterestDto);
      
    }
}
