using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Dtos;
using NotificationService.Enums;
using NotificationService.Hubs;
using NotificationService.Models;
using NotificationService.Repository.NotificationRepository;
using Sprache;
using System.ComponentModel.DataAnnotations;

namespace NotificationService.Services.NotificationService
{
    public class NotificationServices : INotificationService
    {

        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IMapper _mapper;


        public NotificationServices(INotificationRepository notificationRepository , IHubContext<NotificationHub> hubContext , IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _hubContext = hubContext;
            _mapper = mapper;
        }




        public async Task SendNotificaitonToEmployer(InterestRequestDto interestRequestDto)
        {
            try
            {
                if (interestRequestDto.LabourUserId == interestRequestDto.EmployerUserId)
                {

                    throw new ValidationException("Cannot send notification to yourself");
                }

                Notification notification = new Notification()
                {
                    SenderUserId = interestRequestDto.LabourUserId,
                    SenderName = interestRequestDto.LabourName,
                    SenderImageUrl = interestRequestDto.LabourImageUrl,
                    JobPostId = interestRequestDto.JobPostId,
                    ReceiverUserId = interestRequestDto.EmployerUserId,
                    ReceicverName = interestRequestDto.EmployerName,
                    Message = $"{interestRequestDto.LabourName} have sented a job Request to your JobPost",
                    NotificationType = NotificationType.ShowingInterestRequest,
                    IsRead = false

                };

                var result = await _notificationRepository.AddNotification(notification);
                if (!result)
                {
                    throw new Exception("Error when adding notification to the database ");
                }


                var ntfctn = _mapper.Map<NotificationViewDto>(notification);

                if (ntfctn != null)
                {

                    if (NotificationHub.ConnectedUsers.TryGetValue(ntfctn.ReceiverUserId.ToString(), out string connectionId))
                    {
                        await _hubContext.Clients.Client(connectionId)
                            .SendAsync("ReceiveNotification", ntfctn);
                        notification.IsRead = true;
                        await _notificationRepository.UpdateNotification(notification);
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }

        }


        public async Task SendNotificaitonToLabour(AcceptInterestDto acceptInterestDto)
        {
            try
            {

             Notification notification = new Notification()
            {
                SenderUserId = acceptInterestDto.EmployerUserId,
                SenderName = acceptInterestDto.EmployerName,
                SenderImageUrl = acceptInterestDto.EmployerImageUrl,
                JobPostId = acceptInterestDto.JobPostId,
                ReceiverUserId = acceptInterestDto.LabourUserId,
                ReceicverName = acceptInterestDto.LabourName,
                Message = $"{acceptInterestDto.EmployerName} have accepted your interest request",
                NotificationType = NotificationType.RequestAccepted,
                IsRead = false
                
            };
            var result = await _notificationRepository.AddNotification(notification);


            var ntfctn = _mapper.Map<NotificationViewDto>(notification);

            if (ntfctn != null)
            {

                if (NotificationHub.ConnectedUsers.TryGetValue(ntfctn.ReceiverUserId.ToString(), out string connectionId))
                {
                    await _hubContext.Clients.Client(connectionId)
                        .SendAsync("ReceiveNotification", ntfctn);
                    notification.IsRead = true;
                    await _notificationRepository.UpdateNotification(notification);
                }
            }
            }
            catch (Exception ex)
            {
                throw;
            }

        }


    }
}
