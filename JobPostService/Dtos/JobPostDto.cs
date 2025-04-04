﻿using System.ComponentModel.DataAnnotations;

namespace JobPostService.Dtos
{
	public class JobPostDto
	{
	
		[Required]
		[StringLength(40, ErrorMessage = "Skill name cannot exceed 40 characters")]
		public string Title { get; set; }
		[Required]
		public string Description { get; set; }
		[Required]
		public decimal? Wage { get; set; }
		[Required]
		public DateOnly? StartDate { get; set; }
		[Required]
		public DateOnly EndDate { get; set; }
		[Required]
		public string PrefferedTime { get; set; }
		
		[Required]
		public string? MuncipalityName { get; set; }
		
		[Required]
        public string? Skill1Name { get; set; }

        public string? Skill2Name { get; set; }



    }
}
