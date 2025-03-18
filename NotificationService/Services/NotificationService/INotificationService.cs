using NotificationService.Dtos;
using NotificationService.Models;

namespace NotificationService.Services.NotificationService
{
    public interface INotificationService
    {
        Task SendNotificaitonToEmployer(InterestRequestDto interestRequestDto);
        Task SendNotificaitonToLabour(AcceptInterestDto acceptInterestDto);

        //Task<NotificationViewDto> SendNotificationToLabour(InterestRequestDto intrstReqDto);

    }
}
