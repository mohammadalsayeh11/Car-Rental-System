using System.ComponentModel.DataAnnotations;

namespace CentalMvc.Models;

public class Car
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Car name is required.")]
    [StringLength(100, ErrorMessage = "Name can't exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Brand is required.")]
    [StringLength(50, ErrorMessage = "Brand can't exceed 50 characters.")]
    public string Brand { get; set; } = string.Empty;

    [Required(ErrorMessage = "Car type is required.")]
    [StringLength(50)]
    [Display(Name = "Type")]
    public string CarType { get; set; } = string.Empty;

    [Range(1900, 2100, ErrorMessage = "Enter a valid model year.")]
    [Display(Name = "Model Year")]
    public int ModelYear { get; set; }

    [Range(0.0, 100000, ErrorMessage = "Price must be a positive number.")]
    [Display(Name = "Price / Day")]
    public decimal PricePerDay { get; set; }

    [Required, StringLength(50)]
    public string Transmission { get; set; } = "Automatic";

    [Required, StringLength(50)]
    [Display(Name = "Fuel Type")]
    public string FuelType { get; set; } = "Petrol";

    [Range(1, 12, ErrorMessage = "Seats must be between 1 and 12.")]
    public int Seats { get; set; } = 5;

    [StringLength(255)]
    [Display(Name = "Image URL")]
    public string? ImageUrl { get; set; }

    [Display(Name = "Available")]
    public bool IsAvailable { get; set; } = true;

    [StringLength(500, ErrorMessage = "Description can't exceed 500 characters.")]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
