using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NotificationService.Data;
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
        private ApplicationDbContext _dbContext;


        public NotificationServices(INotificationRepository notificationRepository , IHubContext<NotificationHub> hubContext , IMapper mapper , ApplicationDbContext dbContext)
        {
            _notificationRepository = notificationRepository;
            _hubContext = hubContext;
            _mapper = mapper;
            _dbContext = dbContext;
        }




        //public async Task SendNotificaitonToEmployer(InterestRequestDto interestRequestDto)
        //{
        //    try
        //    {
        //        if (interestRequestDto.LabourUserId == interestRequestDto.EmployerUserId)
        //        {

        //            throw new ValidationException("Cannot send notification to yourself");
        //        }

        //        Notification notification = new Notification()
        //        {
        //            SenderUserId = interestRequestDto.LabourUserId,
        //            SenderName = interestRequestDto.LabourName,
        //            SenderImageUrl = interestRequestDto.LabourImageUrl,
        //            JobPostId = interestRequestDto.JobPostId,
        //            ReceiverUserId = interestRequestDto.EmployerUserId,
        //            ReceicverName = interestRequestDto.EmployerName,
        //            Message = $"{interestRequestDto.LabourName} have sented a job Request to your JobPost",
        //            NotificationType = NotificationType.ShowingInterestRequest,
        //            IsRead = false

        //        };

        //        var result = await _notificationRepository.AddNotification(notification);
        //        if (!result)
        //        {
        //            throw new Exception("Error when adding notification to the database ");
        //        }


        //        var ntfctn = _mapper.Map<NotificationViewDto>(notification);

        //        if (ntfctn != null)
        //        {

        //            if (NotificationHub.ConnectedUsers.TryGetValue(ntfctn.ReceiverUserId.ToString(), out string connectionId))
        //            {
        //                await _hubContext.Clients.Client(connectionId)
        //                    .SendAsync("ReceiveNotification", ntfctn);
        //                notification.IsRead = true;
        //                await _notificationRepository.UpdateNotification(notification);
        //            }
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }

        //}


        public async Task SendNotificaitonToEmployer(InterestRequestDto interestRequestDto, Guid userId)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {

                //if (interestRequestDto.LabourUserId == interestRequestDto.EmployerUserId)
                //{
                //    throw new ValidationException("Cannot send notification to yourself");
                //}

                Notification notification = new Notification()
                {
                    SenderUserId = userId,
                    SenderName = interestRequestDto.LabourName,
                    SenderImageUrl = interestRequestDto.LabourImageUrl,
                    JobPostId = interestRequestDto.JobPostId,
                    ReceiverUserId = interestRequestDto.EmployerUserId,
                    ReceicverName = interestRequestDto.EmployerName,
                    Message = $"{interestRequestDto.LabourName} has sent a job request to your JobPost",
                    NotificationType = NotificationType.ShowingInterestRequest,
                    IsRead = false
                };

                var result = await _notificationRepository.AddNotification(notification);

                if (!result)
                {
                    throw new Exception("Error when adding notification to the database");
                }

                var ntfctn = _mapper.Map<NotificationViewDto>(notification);

                if (ntfctn != null)
                {
                    if (NotificationHub.ConnectedUsers.TryGetValue(ntfctn.ReceiverUserId.ToString(), out string connectionId))
                    {
                        try
                        {
                            await _hubContext.Clients.Client(connectionId)
                                .SendAsync("ReceiveNotification", ntfctn);

                            notification.IsRead = true;
                            await _notificationRepository.UpdateNotification(notification);
                        }
                        catch (Exception ex)
                        {
                           
                            throw new Exception("Notification sending failed, rolling back transaction.");
                        }
                    }
                }

                await transaction.CommitAsync(); 
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }



        public async Task SendNotificaitonToLabour(AcceptInterestDto acceptInterestDto, Guid userId)
        {
            if (acceptInterestDto.LabourUserId == userId)
            {
                throw new ValidationException("Cannot send notification to yourself");
            }
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {

             Notification notification = new Notification()
            {
                SenderUserId = userId,
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
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }

        }


    }
}
