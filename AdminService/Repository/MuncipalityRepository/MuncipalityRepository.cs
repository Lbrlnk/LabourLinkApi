using AdminService.Data;
using AdminService.Dtos;
using AdminService.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;

namespace AdminService.Repository.MuncipalityRepository
{
	public class MuncipalityRepository: IMuncipalityRepository
	{
		private readonly AdminDbContext _context;
		
		public MuncipalityRepository(AdminDbContext context)
		{
			_context = context;
		}
		public async Task<List<Muncipality>> GetMuncipalitiesAsync()
		{
			return await _context.Muncipalities.ToListAsync();
		}
		public async Task<Muncipality> AddMuncipalityAsync(Muncipality muncipality)
		{
			_context.Muncipalities.Add(muncipality);
			await _context.SaveChangesAsync();
			return muncipality;
		}
		public async Task<Muncipality> GetMuncipalityByIdAsync(int id)
		{
			var mun=await _context.Muncipalities.FirstOrDefaultAsync(x=>x.MunicipalityId==id);
			return mun; 
		}
		public async Task<bool> UpdateMuncipalityAsync(Muncipality muncipality)
		{
			_context.Muncipalities.Update(muncipality);
			return await _context.SaveChangesAsync() > 0;
		}
		public async Task<List<Muncipality>> GetMuncipalityByStateAsync(string state)
		{
			var muncipalities=await _context.Muncipalities.Where(x=>x.State==state).ToListAsync();
			return muncipalities;
		}
	}
}
