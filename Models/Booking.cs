using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CentalMvc.Models;

public enum BookingStatus
{
    Pending,
    Confirmed,
    Completed,
    Cancelled
}

public class Booking
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Your name is required.")]
    [StringLength(100)]
    [Display(Name = "Full Name")]
    public string CustomerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Enter a valid phone number.")]
    [StringLength(30)]
    public string Phone { get; set; } = string.Empty;

    // Linked Identity user (who made the booking)
    public string? UserId { get; set; }

    [Display(Name = "Car")]
    public int CarId { get; set; }
    public Car? Car { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Pick-up Date")]
    public DateTime PickupDate { get; set; } = DateTime.Today;

    [DataType(DataType.Date)]
    [Display(Name = "Drop-off Date")]
    public DateTime DropoffDate { get; set; } = DateTime.Today.AddDays(1);

    [StringLength(100)]
    [Display(Name = "Pick-up Location")]
    public string? PickupLocation { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    [Display(Name = "Total Price")]
    public decimal TotalPrice { get; set; }

    public BookingStatus Status { get; set; } = BookingStatus.Pending;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
