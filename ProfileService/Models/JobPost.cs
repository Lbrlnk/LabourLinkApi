using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProfileService.Models;

public partial class JobPost
{
    [Key]
    public Guid JobId { get; set; }

    public Guid CleintId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal? Wage { get; set; }

    public DateTime? StartDate { get; set; }

    public string PrefferedTime { get; set; } = null!;

    public int MuncipalityId { get; set; }

    public Guid SkillId1 { get; set; }

    public Guid? SkillId2 { get; set; }

    public string Status { get; set; } = null!;

    public string Image { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }
}
