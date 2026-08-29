using System.ComponentModel.DataAnnotations;

namespace CentalMvc.Models;

public class BlogPost
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(200, ErrorMessage = "Title can't exceed 200 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Author is required.")]
    [StringLength(100)]
    public string Author { get; set; } = "Admin";

    [Range(0, 100000)]
    [Display(Name = "Comments")]
    public int CommentsCount { get; set; } = 0;

    [StringLength(300, ErrorMessage = "Summary can't exceed 300 characters.")]
    public string? Summary { get; set; }

    [Display(Name = "Content")]
    public string? Content { get; set; }

    [StringLength(255)]
    [Display(Name = "Image URL")]
    public string? ImageUrl { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Published Date")]
    public DateTime PublishedDate { get; set; } = DateTime.Today;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
