using Microsoft.EntityFrameworkCore;
using ProfileService.Data;
using ProfileService.Models;
using System.Runtime.CompilerServices;

namespace ProfileService.Repositories.EmployerRepository
{


    public class EmployerRepository : IEmployerRepository
    {


        private readonly LabourLinkProfileDbContext _context;
        public EmployerRepository(LabourLinkProfileDbContext context)
        {
            _context = context;
        }
       public async Task<Employer> AddEmployer(Employer employer)
        {
           await  _context.Employers.AddAsync(employer);
            return employer;
        }
       public  async Task<bool> UpdateEmployer(Employer employer)
        {
            _context.Employers.Update(employer);
            return true;
        }
       public  async Task<Employer> GetEmployerByIdAsync(Guid id)
        {
            return await _context.Employers.FirstOrDefaultAsync(e => e.UserId == id);
        }

        public async Task<bool> UpdateDatabase()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        

    }
}
