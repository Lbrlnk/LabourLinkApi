﻿using JobPostService.Dtos;
using JobPostService.Helpers.ApiResonse;

namespace JobPostService.Services
{
	public interface IJobService
	{
		Task<ApiResponse<string>> AddNewPost(JobPostDto jobPostDto, IFormFile image, Guid userid);
		Task<ApiResponse<List<LabourViewJobPostDto>>> GetJobPost();
        Task<ApiResponse<List<JobPostDtoMinimal>>> GetJobPostactive();
        Task<ApiResponse<JobPostDtoWithLabour>> GetJobPostById(Guid id);
		Task<ApiResponse<string>> UpdateJobPost(UpdatePostDto updatePost, Guid clientid, Guid Jobid);
		Task<ApiResponse<string>> ChangeStatus(string status, Guid jobid, Guid clientid);
		Task<ApiResponse<List<LabourViewJobPostDto>>> GetJobPostByClientid(Guid clientid);
		Task<ApiResponse<List<LabourViewJobPostDto>>> SearchJobPost(string title);
        Task<ApiResponse<List<LabourViewJobPostDto>>> GetJobPostBySkillandMuncipality(string municipality, List<string> skills);
    }
}
 