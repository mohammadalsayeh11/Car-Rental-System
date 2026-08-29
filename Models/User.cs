using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CentalMvc.Models;

// Extends Identity's user with our extra profile fields.
public class ApplicationUser : IdentityUser
{
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(100, ErrorMessage = "Full name can't exceed 100 characters.")]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
