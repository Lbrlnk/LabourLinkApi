using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotificationService.Data;
using NotificationService.Dtos;
using NotificationService.Models;

namespace NotificationService.Repository.InterestRequestRepository
{
    public class InterestRequestRepository : IInterestRequestRepository
    {
        private readonly ApplicationDbContext _context;
        public InterestRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddInterestRequest(InterestRequest intreq)
        {
            try
            {
                await _context.InterestRequests.AddAsync(intreq);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<InterestRequest> GetInterestRequestByEIdAndPId(Guid eId, Guid pId)
        {
            try
            {
                return await _context.InterestRequests.FirstOrDefaultAsync(x => x.EmployerUserId == eId && x.JobPostId == pId);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateInterestRequest(InterestRequest intereq)
        {

            try
            {
                _context.InterestRequests.Update(intereq);
                return await _context.SaveChangesAsync() > 0;

            }
            catch (Exception)
            {
                return false;
            }
        }



        public async Task<InterestRequest> GetInterestRequestById(Guid id)
        {
            try
            {
                return await _context.InterestRequests.FirstOrDefaultAsync(i => i.Id == id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<InterestRequest>> GetAllInterestRequestToEmployer(Guid eId)
        {
            try
            {
                return await _context.InterestRequests
                    .Where(i => (i.Status == Enums.InterestRequestStatus.Accepted || i.Status == Enums.InterestRequestStatus.Pending)
                                && i.EmployerUserId == eId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<InterestRequest>();
            }
        }

        public async Task<List<InterestRequest>> GetAllInterestRequestOfLabour(Guid Lid)
        {
            try
            {
                return await _context.InterestRequests
                    .Where(i => i.LabourUserId == Lid && i.Status == Enums.InterestRequestStatus.Accepted).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<InterestRequest>();
            }
        }

    }
}
