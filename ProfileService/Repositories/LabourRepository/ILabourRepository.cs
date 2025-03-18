using ProfileService.Dtos;
using ProfileService.Models;

namespace ProfileService.Repositories.LabourRepository
{
    public interface ILabourRepository
    {
        Task<bool> AddLabour(Labour lbr);
        Task<bool> AddLabourPreferredMuncipalities(LabourPreferredMuncipality lpm);
        Task<bool> AddLabourWorkImages(LabourWorkImage lwi);

        Task<bool> AddLabourSkills(LabourSkills skill);

        Task<bool> UpdateDatabase();

        Task<Labour> GetLabourByIdAsync(Guid Id);

        Task<List<Labour>> GetFilterdLabours(LabourFilterDto filterDto);
        Task<List<Labour>> GetAllLabours();

        Task<List<Review>> GetLabourReviews(Guid labourId);

        Task<List<LabourWorkImage>> GetLabourWorkImages(Guid Id);
        Task<List<LabourSkills>> GetLabourSkills(Guid Id);
        Task<List<LabourPreferredMuncipality>> GetLabourMuncipalities(Guid Id);
        Task<bool> UpdateLabour(Labour labour);

        Task<Labour> GetLabourByPhone(string phoneNumber);

        Task<Labour> GetMyDetails(Guid id);


    }
}
 