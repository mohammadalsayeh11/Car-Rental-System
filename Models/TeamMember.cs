using System.ComponentModel.DataAnnotations;

namespace CentalMvc.Models;

public class TeamMember
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Role / profession is required.")]
    [StringLength(100)]
    public string Role { get; set; } = string.Empty;

    [StringLength(255)]
    [Display(Name = "Image URL")]
    public string? ImageUrl { get; set; }

    [StringLength(255)]
    public string? FacebookUrl { get; set; }

    [StringLength(255)]
    public string? TwitterUrl { get; set; }

    [StringLength(255)]
    public string? InstagramUrl { get; set; }

    [StringLength(255)]
    public string? LinkedInUrl { get; set; }

    [Display(Name = "Display Order")]
    public int DisplayOrder { get; set; } = 0;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
