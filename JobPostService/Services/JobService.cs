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
		public JobService(IJobRepository repository,ICloudinaryHelper cloudinary,IMapper mapper)
		{
			_repository = repository;
			_cloudinary = cloudinary;    
			_mapper = mapper;
		}
		public async Task<ApiResponse<string>> AddNewPost(JobPostDto jobPostDto,IFormFile image)
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
					
					CleintId=jobPostDto.ClientId,
					Title=jobPostDto.Title,
					Description=jobPostDto.Description,
					Wage=jobPostDto.Wage,
					StartDate=jobPostDto.StartDate,
					SkillId1=jobPostDto.SkillId1,
					Skill1=jobPostDto.Skill1,
					Skill2=jobPostDto.Skill2,
					Muncipality=jobPostDto.Muncipality,
					SkillId2=jobPostDto.SkillId2,
					PrefferedTime=jobPostDto.PrefferedTime,
					MuncipalityId=jobPostDto.MuncipalityId,
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
		public async Task<ApiResponse<List<LabourViewJobPostDto>>> GetJobPostactive()
		{
			var result = await _repository.GetPostAsyncActiveAsync();
			if (!result.Any())
			{
				return new ApiResponse<List<LabourViewJobPostDto>>(404, "There is no JobPost ");
			}
			var res = _mapper.Map<List<LabourViewJobPostDto>>(result);
			return new ApiResponse<List<LabourViewJobPostDto>>(200, "success", res);
		}
		public async Task<ApiResponse<LabourViewJobPostDto>> GetJobPostById(Guid id)
		{
			try
			{
				var result=await _repository.GetJobPostByIdAsync(id);
				if (result==null)
				{
					return new ApiResponse<LabourViewJobPostDto>(404, "There is no JobPost in this id");
				}
				var res = _mapper.Map<LabourViewJobPostDto>(result);
				return new ApiResponse<LabourViewJobPostDto>(200, "success", res);
			}catch (Exception ex)
			{
				return new ApiResponse<LabourViewJobPostDto>(500, "Something Went Wrong");
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
				existingJob.PrefferedTime = updatePost.PrefferedTime ?? existingJob.PrefferedTime;
				existingJob.MuncipalityId = updatePost.MuncipalityId ?? existingJob.MuncipalityId;
				existingJob.SkillId1 = updatePost.SkillId1 ?? existingJob.SkillId1;
				existingJob.SkillId2= updatePost.SkillId2 ?? existingJob.SkillId2;
				existingJob.Muncipality= updatePost.Muncipality?? existingJob.Muncipality;
				existingJob.Skill1=updatePost.Skill1??existingJob.Skill1;
				existingJob.Skill2 = updatePost.Skill2 ?? existingJob.Skill2;

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
		public async Task<ApiResponse<List<LabourViewJobPostDto>>> FilterJopPostBasedOnSkill(Guid Skillid)
		{
			try
			{
				var result=await _repository.FilterJopPostBasedOnSkillAsync(Skillid);
				if (!result.Any())
				{
					return new ApiResponse<List<LabourViewJobPostDto>>(404, "there is no job post in this skill");
				}
				var res = _mapper.Map<List<LabourViewJobPostDto>>(result);
				return new ApiResponse<List<LabourViewJobPostDto>>(200, "success", res);
			}
			catch (Exception ex)
			{
				return new ApiResponse<List<LabourViewJobPostDto>>(500, ex.Message);
			}
		}
		public async Task<ApiResponse<List<LabourViewJobPostDto>>> FilterJopPostBasedOnMuncipality(int muncipalityid)
		{
			try
			{
				var result = await _repository.FilterJobPostBasedOnMuncipalityAsync(muncipalityid);
				if (!result.Any())
				{
					return new ApiResponse<List<LabourViewJobPostDto>>(404, "there is no job post in this muncipality");
				}
				var res = _mapper.Map<List<LabourViewJobPostDto>>(result);
				return new ApiResponse<List<LabourViewJobPostDto>>(200, "success", res);
			}
			catch (Exception ex)
			{
				return new ApiResponse<List<LabourViewJobPostDto>>(500, ex.Message);
			}
		}
	}
}
