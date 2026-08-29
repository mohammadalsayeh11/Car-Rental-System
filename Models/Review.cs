using System.ComponentModel.DataAnnotations;

namespace CentalMvc.Models;

public class Review
{
    public int Id { get; set; }

    public int CarId { get; set; }
    public Car? Car { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;   // Identity user id

    [Required, StringLength(100)]
    public string UserName { get; set; } = string.Empty;

    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
    public int Rating { get; set; } = 5;

    [Required(ErrorMessage = "Please write your review.")]
    [StringLength(500, ErrorMessage = "Review can't exceed 500 characters.")]
    public string Comment { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
