using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NotificationService.Dtos;
using NotificationService.Hubs;
using NotificationService.Mapper;
using NotificationService.Models;
using NotificationService.Repository.InterestRequestRepository;
using NotificationService.Services.NotificationService;

namespace NotificationService.Services.IntrestRequestService
{
    //    public class InterestRequestService : IInterestRequestService

    //    {
    //        private readonly IInterestRequestRepository _interestRequestRepository;
    //        private readonly IMapper _mapper;
    //        private readonly INotificationService _notificationService;

    //        public InterestRequestService(IInterestRequestRepository interestRequestRepository , IMapper mapper , INotificationService notificationService ) 
    //        {
    //            _interestRequestRepository = interestRequestRepository;
    //            _mapper = mapper;
    //            _notificationService = notificationService;

    //        }
    //        public async Task<string> AddInterestRequest(InterestRequestDto interestRequestDto)
    //        {
    //            try
    //            {
    //                var isAlreadyReq = await _interestRequestRepository.GetInterestRequestByEIdAndPId(interestRequestDto.EmployerUserId ,interestRequestDto.JobPostId);
    //                if(isAlreadyReq != null && isAlreadyReq.Status == Enums.InterestRequestStatus.Pending)
    //                {
    //                    return "Error: Interest request already exist.";
    //                }
    //                var req = _mapper.Map<InterestRequest>(interestRequestDto);
    //                var result = await _interestRequestRepository.AddInterestRequest(req);
    //               await _notificationService.SendNotificaitonToEmployer(interestRequestDto);

    //                if (!result)
    //                {
    //                    throw new Exception("internal server error when adding ");
    //                }
    //                return "interest sented "; 
    //            }
    //            catch (Exception ex)
    //            {
    //                throw;
    //            }
    //        }

    //        public async Task<string> WithdrawInterstRequest(Guid id)
    //        {
    //            try
    //            {
    //               var interestRequest =  await _interestRequestRepository.GetInterestRequestById(id);
    //                if (interestRequest == null)
    //                {
    //                    return "Error: Interest request not found.";
    //                }
    //                if (interestRequest.IsDelete)
    //                {
    //                    return "Error: This request is already deleted.";
    //                }

    //                interestRequest.IsDelete = true;
    //                interestRequest.Status = Enums.InterestRequestStatus.withdrawn;
    //                interestRequest.UpdatedOn =DateTime.UtcNow;
    //                var result =  await _interestRequestRepository.UpdateInterestRequest(interestRequest);

    //                if (!result)
    //                {
    //                    throw new Exception("error happend when updating the changes to database");
    //                }
    //                    return "Request withdrawn successfully";
    //            }
    //            catch (Exception ex)
    //            {
    //                throw;
    //            }
    //        }
    //       public async Task<string> RejectInterestRequest(Guid id)
    //        {
    //            try
    //            {
    //               var interestRequest =  await _interestRequestRepository.GetInterestRequestById(id);
    //                if (interestRequest == null)
    //                {
    //                    return "Error: Interest request not found.";
    //                }
    //                if (interestRequest.IsDelete)
    //                {
    //                    return "Error: This request is already deleted.";
    //                }
    //                if (interestRequest.Status == Enums.InterestRequestStatus.Rejected)
    //                {
    //                    return "Error: This request is already rejected.";
    //                }
    //                interestRequest.Status = Enums.InterestRequestStatus.Rejected;
    //                interestRequest.IsDelete = true ;
    //                interestRequest.UpdatedOn = DateTime.UtcNow;
    //                var result =  await _interestRequestRepository.UpdateInterestRequest(interestRequest);

    //                if (!result)
    //                {
    //                    throw new Exception("error happend when updating the changes to database");
    //                }

    //                    return "Request Rejected successfully";
    //            }
    //            catch (Exception ex)
    //            {
    //                throw;
    //            }
    //        }
    //        public async Task<string> AcceptInterestRequest(AcceptInterestDto acceptInterestDto)
    //        {
    //            try
    //            {
    //                var interestRequest = await _interestRequestRepository.GetInterestRequestById(acceptInterestDto.InterestRequestId);

    //                if (interestRequest == null)
    //                {
    //                    return "Error: Interest request not found.";
    //                }
    //                if (interestRequest.IsDelete)
    //                {
    //                    return "Error: This request is already deleted.";
    //                }
    //                if (interestRequest.Status == Enums.InterestRequestStatus.Rejected)
    //                {
    //                    return "Error: This request is already rejected.";
    //                }

    //                interestRequest.Status = Enums.InterestRequestStatus.Accepted;
    //                interestRequest.UpdatedOn = DateTime.UtcNow;
    //                var result = await _interestRequestRepository.UpdateInterestRequest(interestRequest);

    //                await _notificationService.SendNotificaitonToLabour(acceptInterestDto);

    //                if (!result)
    //                {
    //                    throw new Exception("error happend when updating the changes to database");
    //                }



    //                return "Request Accepted successfully";
    //            }
    //            catch (Exception ex)
    //            {
    //                throw;
    //            }
    //        }

    //        public async Task<List<InterestRequest>> GetInterestRequestForEmployers(Guid eId)
    //        {
    //            try
    //            {

    //                var interestRequestList = await _interestRequestRepository.GetAllInterestRequestToEmployer(eId);


    //                return interestRequestList ?? new List<InterestRequest>();
    //            }
    //            catch (Exception ex)
    //            {

    //                Console.WriteLine($"Error fetching interest requests: {ex.Message}");

    //                return new List<InterestRequest>();
    //            }
    //        }

    //        public async Task<List<InterestRequest>> GetAcceptedInterestRequestOfLabour(Guid Lid)
    //        {
    //            try
    //            {

    //            var acceptedInterestRequestList = await _interestRequestRepository.GetAllInterestRequestOfLabour(Lid);
    //            return acceptedInterestRequestList ?? new List<InterestRequest>();
    //            }
    //         catch (Exception ex)
    //            {

    //                Console.WriteLine($"Error fetching interest requests: {ex.Message}");

    //                return new List<InterestRequest>();
    //            }
    //        }
    //}









    public class InterestRequestService : IInterestRequestService

    {
        private readonly IInterestRequestRepository _interestRequestRepository;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public InterestRequestService(IInterestRequestRepository interestRequestRepository, IMapper mapper, INotificationService notificationService)
        {
            _interestRequestRepository = interestRequestRepository;
            _mapper = mapper;
            _notificationService = notificationService;

        }
        public async Task<string> AddInterestRequest(InterestRequestDto interestRequestDto, Guid userId)
        {
            try
            {
                var isAlreadyReq = await _interestRequestRepository.GetInterestRequestByEIdAndPId(interestRequestDto.EmployerUserId, interestRequestDto.JobPostId);
                if (isAlreadyReq != null && isAlreadyReq.Status == Enums.InterestRequestStatus.Pending)
                {
                    return "Error: Interest request already exist.";
                }
                var req = _mapper.Map<InterestRequest>(interestRequestDto);
                req.LabourUserId = userId;
                var result = await _interestRequestRepository.AddInterestRequest(req);
                await _notificationService.SendNotificaitonToEmployer(interestRequestDto, userId);

                if (!result)
                {
                    throw new Exception("internal server error when adding ");
                }
                return "interest sented ";
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<string> WithdrawInterstRequest(Guid id)
        {
            try
            {
                var interestRequest = await _interestRequestRepository.GetInterestRequestById(id);
                if (interestRequest == null)
                {
                    return "Error: Interest request not found.";
                }
                if (interestRequest.IsDelete)
                {
                    return "Error: This request is already deleted.";
                }

                interestRequest.IsDelete = true;
                interestRequest.Status = Enums.InterestRequestStatus.withdrawn;
                interestRequest.UpdatedOn = DateTime.UtcNow;
                var result = await _interestRequestRepository.UpdateInterestRequest(interestRequest);

                if (!result)
                {
                    throw new Exception("error happend when updating the changes to database");
                }
                return "Request withdrawn successfully";
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<string> RejectInterestRequest(Guid id)
        {
            try
            {
                var interestRequest = await _interestRequestRepository.GetInterestRequestById(id);
                if (interestRequest == null)
                {
                    return "Error: Interest request not found.";
                }
                if (interestRequest.IsDelete)
                {
                    return "Error: This request is already deleted.";
                }
                if (interestRequest.Status == Enums.InterestRequestStatus.Rejected)
                {
                    return "Error: This request is already rejected.";
                }
                interestRequest.Status = Enums.InterestRequestStatus.Rejected;
                interestRequest.IsDelete = true;
                interestRequest.UpdatedOn = DateTime.UtcNow;
                var result = await _interestRequestRepository.UpdateInterestRequest(interestRequest);

                if (!result)
                {
                    throw new Exception("error happend when updating the changes to database");
                }

                return "Request Rejected successfully";
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<string> AcceptInterestRequest(AcceptInterestDto acceptInterestDto, Guid userId)
        {
            try
            {
                var interestRequest = await _interestRequestRepository.GetInterestRequestById(acceptInterestDto.InterestRequestId);

                if (interestRequest == null)
                {
                    return "Error: Interest request not found.";
                }
                if (interestRequest.IsDelete)
                {
                    return "Error: This request is already deleted.";
                }
                if (interestRequest.Status == Enums.InterestRequestStatus.Rejected)
                {
                    return "Error: This request is already rejected.";
                }
                interestRequest.Status = Enums.InterestRequestStatus.Accepted;
                interestRequest.UpdatedOn = DateTime.UtcNow;

                var result = await _interestRequestRepository.UpdateInterestRequest(interestRequest);

                await _notificationService.SendNotificaitonToLabour(acceptInterestDto, userId);

                if (!result)
                {
                    throw new Exception("error happend when updating the changes to database");
                }
                return "Request Accepted successfully";
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<List<InterestRequest>> GetInterestRequestForEmployers(Guid eId)
        {
            try
            {
                var interestRequestList = await _interestRequestRepository.GetAllInterestRequestToEmployer(eId);
                return interestRequestList ?? new List<InterestRequest>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching interest requests: {ex.Message}");
                return new List<InterestRequest>();
            }
        }

        public async Task<List<InterestRequest>> GetAcceptedInterestRequestOfLabour(Guid Lid)
        {
            try
            {
                var acceptedInterestRequestList = await _interestRequestRepository.GetAllInterestRequestOfLabour(Lid);
                return acceptedInterestRequestList ?? new List<InterestRequest>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching interest requests: {ex.Message}");
                return new List<InterestRequest>();
            }
        }


    }
}
