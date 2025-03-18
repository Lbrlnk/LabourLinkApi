using NotificationService.Dtos;
using NotificationService.Models;

namespace NotificationService.Repository.InterestRequestRepository
{
    public interface IInterestRequestRepository
    {
        Task<bool> AddInterestRequest(InterestRequest intreq);
        Task<bool> UpdateInterestRequest(InterestRequest intereq);

        Task<InterestRequest> GetInterestRequestById(Guid id);

        Task<InterestRequest> GetInterestRequestByEIdAndPId(Guid eId, Guid pId);
    }
}
