using System.ComponentModel.DataAnnotations;

namespace CentalMvc.Models;

public class AboutInfo
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string Heading { get; set; } = "About Us";

    [Required, StringLength(1000)]
    public string Intro { get; set; } = string.Empty;

    [StringLength(150)]
    [Display(Name = "Vision Title")]
    public string VisionTitle { get; set; } = "Our Vision";

    [StringLength(500)]
    [Display(Name = "Vision Text")]
    public string VisionText { get; set; } = string.Empty;

    [StringLength(150)]
    [Display(Name = "Mission Title")]
    public string MissionTitle { get; set; } = "Our Mission";

    [StringLength(500)]
    [Display(Name = "Mission Text")]
    public string MissionText { get; set; } = string.Empty;

    [StringLength(1000)]
    [Display(Name = "Body Text")]
    public string BodyText { get; set; } = string.Empty;

    [Range(0, 200)]
    [Display(Name = "Years Of Experience")]
    public int YearsOfExperience { get; set; } = 17;

    // Feature bullet points, one per line
    [StringLength(1000)]
    [Display(Name = "Features (one per line)")]
    public string Features { get; set; } = string.Empty;

    [StringLength(100)]
    [Display(Name = "Founder Name")]
    public string FounderName { get; set; } = string.Empty;

    [StringLength(100)]
    [Display(Name = "Founder Title")]
    public string FounderTitle { get; set; } = string.Empty;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
