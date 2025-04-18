﻿using ProfileService.Models;

namespace ProfileService.Repositories.EmployerRepository
{
    public interface IEmployerRepository
    {
        Task<Employer> AddEmployer(Employer employer);
        Task<bool> UpdateEmployer(Employer employer);
        Task<Employer> GetEmployerByIdAsync(Guid Id);
        Task<List<Employer>> GetAllEmployersAsync();
		Task<bool> UpdateDatabase();
        Task<int> CountEmployersAsync();


	}
}
