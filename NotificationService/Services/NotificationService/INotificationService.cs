using NotificationService.Dtos;
using NotificationService.Models;

namespace NotificationService.Services.NotificationService
{
    public interface INotificationService
    {
        Task SendNotificaitonToEmployer(InterestRequestDto interestRequestDto,Guid userId);
        Task SendNotificaitonToLabour(AcceptInterestDto acceptInterestDto,Guid userId);

        //Task<NotificationViewDto> SendNotificationToLabour(InterestRequestDto intrstReqDto);

    }
}
