using AutoMapper;
using JobPostService.Dtos;
using JobPostService.Helpers.ApiResonse;
using JobPostService.Helpers.CloudinaryHelper;
using JobPostService.Models;
using JobPostService.Repository;
using Microsoft.VisualBasic;
using Sprache;

namespace JobPostService.Services
{
	public class JobService: IJobService
	{
		private readonly IJobRepository _repository;
		private readonly ICloudinaryHelper _cloudinary;
		private readonly IMapper _mapper;
		private readonly ProfileServiceClient _profileServiceClient;
		public JobService(IJobRepository repository,ICloudinaryHelper cloudinary,IMapper mapper, ProfileServiceClient profileServiceClient)
		{
			_repository = repository;
			_cloudinary = cloudinary;
			_mapper = mapper;
			_profileServiceClient = profileServiceClient;
		}
		public async Task<ApiResponse<string>> AddNewPost(JobPostDto jobPostDto,IFormFile image,Guid userid)
		{
			try
			{
				if (jobPostDto == null)
				{
					return new ApiResponse<string>(400, "Job post data is required.");
				}
				if (image == null || image.Length == 0)
				{
					return new ApiResponse<string>(400, "Image file is required.");
				}


				string imageurl = await _cloudinary.UploadImage(image);
				Console.WriteLine(imageurl);
				JobPost jobPost = new JobPost
				{
					CleintId=userid,
					Title=jobPostDto.Title,
					Description=jobPostDto.Description,
					Wage=jobPostDto.Wage,
					StartDate=jobPostDto.StartDate,
					EndDate=jobPostDto.EndDate,
					PrefferedTime=jobPostDto.PrefferedTime,
					MuncipalityId=jobPostDto.MuncipalityId,
					SkillId1=jobPostDto.SkillId1,
					SkillId2=jobPostDto.SkillId2,
					Image=imageurl
				};
				var res=await _repository.AddJobPost(jobPost);
					return new ApiResponse<string>(201, "success", "Job post created Successfully");
			}catch (Exception ex)
			{
				return new  ApiResponse<string>(500, ex.Message);
			}
		}
		public async Task<ApiResponse<List<LabourViewJobPostDto>>> GetJobPost()
		{
			var result=await _repository.GetJobPostsAsync();
			if (!result.Any())
			{
				return new ApiResponse<List<LabourViewJobPostDto>>(404, "There is no JobPost ");
			}
			var res=_mapper.Map<List<LabourViewJobPostDto>>(result);
			return new ApiResponse<List<LabourViewJobPostDto>>(200,"success",res);
		}
		public async Task<ApiResponse<List<JobPostDtoMinimal>>> GetJobPostactive()
		{
			var jobPosts = await _repository.GetPostAsyncActiveAsync();
			if (!jobPosts.Any())
			{
				return new ApiResponse<List<JobPostDtoMinimal>>(404, "There is no JobPost.");
			}

			var jobPostDtos = new List<JobPostDtoMinimal>();

			foreach (var job in jobPosts)
			{
				var client = await _profileServiceClient.GetClientByIdAsync(job.CleintId);
				if (client.StatusCode != 200 || client.Data == null)
				{
					return new ApiResponse<List<JobPostDtoMinimal>>(404, "Client not found.");
				}

				var jobPostDto = new JobPostDtoMinimal
				{
					Title = job.Title,
					Description = job.Description,
					Wage = job.Wage,
					StartDate = job.StartDate,
					EndDate = job.EndDate,
					PrefferedTime = job.PrefferedTime,
					MuncipalityId = job.MuncipalityId,
					Status = job.Status,
					SkillId1 = job.SkillId1,
					SkillId2 = job.SkillId2,
					Image = job.Image,
					CreatedDate = job.CreatedDate,
					FullName = client.Data.FullName,
					ProfileImageUrl = client.Data.ProfileImageUrl,
				};

				jobPostDtos.Add(jobPostDto);
			}

			return new ApiResponse<List<JobPostDtoMinimal>>(200, "success", jobPostDtos);
		}

		public async Task<ApiResponse<JobPostDtoWithLabour>> GetJobPostById(Guid id)
		{
			try
			{
				var result = await _repository.GetJobPostByIdAsync(id);
				if (result == null)
				{
					return new ApiResponse<JobPostDtoWithLabour>(404, "No job post found with the given ID.");
				}

				var client = await _profileServiceClient.GetClientByIdAsync(result.CleintId);
				if (client.StatusCode != 200 || client.Data == null)
				{
					return new ApiResponse<JobPostDtoWithLabour>(404, "Client not found.");
				}

				var res = new JobPostDtoWithLabour
				{
					Title = result.Title,
					Description = result.Description,
					Wage = result.Wage,
					StartDate = result.StartDate,
					EndDate = result.EndDate,
					PrefferedTime = result.PrefferedTime,
					MuncipalityId = result.MuncipalityId,
					Status = result.Status,
					SkillId1 = result.SkillId1,
					SkillId2 = result.SkillId2,
					Image = result.Image,
					CreatedDate = result.CreatedDate,
					FullName = client.Data.FullName,
					ProfileImageUrl = client.Data.ProfileImageUrl,
					PreferedMunicipality = client.Data.PreferedMunicipality 
				};

				return new ApiResponse<JobPostDtoWithLabour>(200, "Success", res);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error in GetJobPostById: {ex.Message}");
				return new ApiResponse<JobPostDtoWithLabour>(500, "Something went wrong.");
			}
		}

		public async Task<ApiResponse<string>> UpdateJobPost(UpdatePostDto updatePost, Guid clientid, Guid Jobid)
		{
			try
			{
				if (updatePost == null)
				{
					return new ApiResponse<string>(400, "Update data is required.");
				}
				var existingJob = await _repository.GetJobPostByIdAsync(Jobid);
				if (existingJob == null)
				{
					return new ApiResponse<string>(404, "Job post not found.");
				}
				if (existingJob.CleintId != clientid)
				{
					return new ApiResponse<string>(403, "Unauthorized to update this job post.");
				}

				existingJob.Title = updatePost.Title ?? existingJob.Title;
				existingJob.Description = updatePost.Description ?? existingJob.Description;
				existingJob.Wage = updatePost.Wage ?? existingJob.Wage;
				existingJob.StartDate = updatePost.StartDate ?? existingJob.StartDate;
				existingJob.EndDate = updatePost.EndDate ?? existingJob.EndDate;
				existingJob.PrefferedTime = updatePost.PrefferedTime ?? existingJob.PrefferedTime;
				existingJob.MuncipalityId = updatePost.MuncipalityId ?? existingJob.MuncipalityId;
				existingJob.SkillId1 = updatePost.SkillId1 ?? existingJob.SkillId1;
				existingJob.SkillId2= updatePost.SkillId2 ?? existingJob.SkillId2;

				
				var result = await _repository.UpdatePostAsync(existingJob);
				if (result)
				{
					return new ApiResponse<string>(200, "Job post updated successfully.");
				}
				return new ApiResponse<string>(500, "Failed to update job post.");
			}
			catch (Exception ex)
			{
				return new ApiResponse<string>(500, ex.Message);
			}
		}
		public async Task<ApiResponse<string>> ChangeStatus(string status,Guid jobid,Guid clientid)
		{
			if(status == null)
			{
				return new ApiResponse<string>(400, "updated status is required ");
			}
			var existingJob = await _repository.GetJobPostByIdAsync(jobid);
			if (existingJob == null)
			{
				return new ApiResponse<string>(404, "Job post not found.");
			}
			if (existingJob.CleintId != clientid)
			{
				return new ApiResponse<string>(403, "Unauthorized to update this job post.");
			}
			existingJob.Status = status;
			var result = await _repository.UpdatePostAsync(existingJob);
			if (result)
			{
				return new ApiResponse<string>(200, "Job post updated successfully.");
			}
			return new ApiResponse<string>(500, "Failed to update job post.");

		}
		public async Task<ApiResponse<List<LabourViewJobPostDto>>> GetJobPostByClientid(Guid clientid)
		{
			try
			{
				var result =await _repository.GetJobPostByClientAsync(clientid);
				if (!result.Any())
				{
					return new ApiResponse<List<LabourViewJobPostDto>>(404, "there is no post for this cleint");
				}
				var res=_mapper.Map<List<LabourViewJobPostDto>>(result);
				return new ApiResponse<List<LabourViewJobPostDto>>(200, "success", res);
			}catch (Exception ex) { 
				return new ApiResponse<List<LabourViewJobPostDto>>(500, ex.Message);
			}
		}
		public async Task<ApiResponse<List<LabourViewJobPostDto>>> SearchJobPost(string title)
		{
			try
			{
				if(title == null)
				{
					return new ApiResponse<List<LabourViewJobPostDto>>(400, "search something");
				}
				var result = await _repository.GetJobPostBySearchParamsAsync(title);
				if (!result.Any())
				{
					return new ApiResponse<List<LabourViewJobPostDto>>(404, "there is no job post in this given word");
					}
				var res = _mapper.Map<List<LabourViewJobPostDto>>(result);
				return new ApiResponse<List<LabourViewJobPostDto>>(200, "success", res);
			}
			catch (Exception ex)
			{
				return new ApiResponse<List<LabourViewJobPostDto>>(500,ex.Message);
			}
		}
		public async Task<ApiResponse<List<LabourViewJobPostDto>>> GetJobPostBySkillandMuncipality(string municipality, List<string> skills)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(municipality) || skills == null || skills.Count == 0)
				{
					return new ApiResponse<List<LabourViewJobPostDto>>(400, "Municipality and at least one skill are required.");
				}

				var jobPosts = await _repository.GetJobPostBySkillandMuncipalityAsync(municipality, skills);

				if (jobPosts.Count == 0)
				{
					return new ApiResponse<List<LabourViewJobPostDto>>(404, "No job posts found matching the criteria.");
				}

				var mappedResult = _mapper.Map<List<LabourViewJobPostDto>>(jobPosts);
				return new ApiResponse<List<LabourViewJobPostDto>>(200, "Success", mappedResult);
			}
			catch (Exception ex)
			{
				return new ApiResponse<List<LabourViewJobPostDto>>(500, "An error occurred while processing your request.");
			}
		}
	}
}
