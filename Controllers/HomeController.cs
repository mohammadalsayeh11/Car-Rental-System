using CentalMvc.Data;
using CentalMvc.Models;
using CentalMvc.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CentalMvc.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(AppDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var cars = await _db.Cars
            .Where(c => c.IsAvailable)
            .OrderByDescending(c => c.Id)
            .Take(8)
            .ToListAsync();

        var carIds = cars.Select(c => c.Id).ToList();

        // Build average rating + count per car in one query
        var ratings = await _db.Reviews
            .Where(r => carIds.Contains(r.CarId))
            .GroupBy(r => r.CarId)
            .Select(g => new { CarId = g.Key, Avg = g.Average(x => x.Rating), Count = g.Count() })
            .ToListAsync();

        var ratingMap = ratings.ToDictionary(
            r => r.CarId,
            r => (Avg: Math.Round(r.Avg, 1), Count: r.Count));

        var blogs = await _db.BlogPosts
            .OrderByDescending(b => b.PublishedDate)
            .Take(3)
            .ToListAsync();

        var team = await _db.TeamMembers
            .Where(t => t.IsActive)
            .OrderBy(t => t.DisplayOrder)
            .ToListAsync();

        var vm = new HomeViewModel
        {
            FeaturedCars = cars,
            CarRatings = ratingMap,
            BlogPosts = blogs,
            Team = team
        };

        return View(vm);
    }

    public async Task<IActionResult> About()
    {
        var info = await _db.AboutInfos.FirstOrDefaultAsync() ?? new AboutInfo();
        var team = await _db.TeamMembers
            .Where(t => t.IsActive)
            .OrderBy(t => t.DisplayOrder)
            .ToListAsync();

        var vm = new AboutViewModel { Info = info, Team = team };
        return View(vm);
    }

    // ---------- BLOG ----------
    public async Task<IActionResult> Blog()
    {
        var blogs = await _db.BlogPosts
            .OrderByDescending(b => b.PublishedDate)
            .ToListAsync();
        return View(blogs);
    }

    public async Task<IActionResult> BlogDetails(int id)
    {
        var post = await _db.BlogPosts.FirstOrDefaultAsync(b => b.Id == id);
        if (post == null) return NotFound();
        return View(post);
    }

    public async Task<IActionResult> Vehicle()
    {
        var cars = await _db.Cars.OrderByDescending(c => c.Id).ToListAsync();
        return View(cars);
    }

    public async Task<IActionResult> Team()
    {
        var team = await _db.TeamMembers
            .Where(t => t.IsActive)
            .OrderBy(t => t.DisplayOrder)
            .ToListAsync();
        return View(team);
    }

    // ---------- CONTACT ----------
    [HttpGet]
    public IActionResult Contact() => View(new ContactMessage());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Contact(ContactMessage message)
    {
        if (!ModelState.IsValid) return View(message);
        message.CreatedAt = DateTime.UtcNow;
        _db.ContactMessages.Add(message);
        await _db.SaveChangesAsync();
        TempData["Msg"] = "Thank you! Your message has been sent.";
        return RedirectToAction(nameof(Contact));
    }

    // ---------- CAR DETAILS + REVIEWS ----------
    public async Task<IActionResult> CarDetails(int id)
    {
        var car = await _db.Cars.FirstOrDefaultAsync(c => c.Id == id);
        if (car == null) return NotFound();

        ViewBag.Reviews = await _db.Reviews
            .Where(r => r.CarId == id)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        var ratings = await _db.Reviews.Where(r => r.CarId == id).Select(r => r.Rating).ToListAsync();
        ViewBag.AvgRating = ratings.Count > 0 ? Math.Round(ratings.Average(), 1) : 0;
        ViewBag.ReviewCount = ratings.Count;

        return View(car);
    }

    [HttpPost]
    [Authorize]                       // only logged-in users can review
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddReview(int carId, int rating, string comment)
    {
        var car = await _db.Cars.FindAsync(carId);
        if (car == null) return NotFound();

        if (string.IsNullOrWhiteSpace(comment))
        {
            TempData["Err"] = "Please write your review before submitting.";
            return RedirectToAction(nameof(CarDetails), new { id = carId });
        }

        var user = await _userManager.GetUserAsync(User);
        var review = new Review
        {
            CarId = carId,
            UserId = user!.Id,
            UserName = user.FullName,
            Rating = Math.Clamp(rating, 1, 5),
            Comment = comment.Trim(),
            CreatedAt = DateTime.UtcNow
        };
        _db.Reviews.Add(review);
        await _db.SaveChangesAsync();
        TempData["Msg"] = "Thanks for your review!";
        return RedirectToAction(nameof(CarDetails), new { id = carId });
    }

    // ---------- QUICK BOOK (from home hero form) ----------
    // Anyone can submit; sends them to the Book page (which requires login).
    [HttpGet]
    public IActionResult QuickBook(int carId, string? pickupLocation, DateTime? pickupDate, DateTime? dropoffDate)
    {
        if (carId <= 0)
        {
            TempData["Err"] = "Please select a car first.";
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(Book), new
        {
            id = carId,
            pickupLocation,
            pickupDate = pickupDate?.ToString("yyyy-MM-dd"),
            dropoffDate = dropoffDate?.ToString("yyyy-MM-dd")
        });
    }

    // ---------- BOOKING (login required) ----------
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Book(int id, string? pickupLocation = null, DateTime? pickupDate = null, DateTime? dropoffDate = null)
    {
        var car = await _db.Cars.FindAsync(id);
        if (car == null) return NotFound();
        if (!car.IsAvailable)
        {
            TempData["Err"] = "Sorry, this car is not available right now.";
            return RedirectToAction(nameof(Vehicle));
        }

        var user = await _userManager.GetUserAsync(User);
        var booking = new Booking
        {
            CarId = car.Id,
            Car = car,
            CustomerName = user?.FullName ?? "",
            Email = user?.Email ?? "",
            Phone = user?.PhoneNumber ?? "",
            PickupLocation = pickupLocation,
            PickupDate = pickupDate?.Date ?? DateTime.Today,
            DropoffDate = dropoffDate?.Date ?? DateTime.Today.AddDays(1)
        };
        return View(booking);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Book(Booking booking)
    {
        var car = await _db.Cars.FindAsync(booking.CarId);
        if (car == null) return NotFound();

        if (booking.DropoffDate <= booking.PickupDate)
            ModelState.AddModelError(nameof(Booking.DropoffDate), "Drop-off must be after pick-up.");

        if (booking.PickupDate.Date < DateTime.Today)
            ModelState.AddModelError(nameof(Booking.PickupDate), "Pick-up date can't be in the past.");

        if (!ModelState.IsValid)
        {
            booking.Car = car;
            return View(booking);
        }

        var user = await _userManager.GetUserAsync(User);
        var days = Math.Max(1, (booking.DropoffDate - booking.PickupDate).Days);

        // Build a clean entity so EF treats Id as identity (auto-generated).
        var newBooking = new Booking
        {
            CustomerName = booking.CustomerName,
            Email = booking.Email,
            Phone = booking.Phone,
            CarId = booking.CarId,
            UserId = user?.Id,
            PickupDate = booking.PickupDate,
            DropoffDate = booking.DropoffDate,
            PickupLocation = booking.PickupLocation,
            TotalPrice = days * car.PricePerDay,
            Status = BookingStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _db.Bookings.Add(newBooking);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(BookingConfirmed), new { id = newBooking.Id });
    }

    [Authorize]
    public async Task<IActionResult> BookingConfirmed(int id)
    {
        var booking = await _db.Bookings.Include(b => b.Car)
            .FirstOrDefaultAsync(b => b.Id == id);
        if (booking == null) return NotFound();
        return View(booking);
    }

    // ---------- MY BOOKINGS (customer) ----------
    [Authorize]
    public async Task<IActionResult> MyBookings()
    {
        var user = await _userManager.GetUserAsync(User);
        var bookings = await _db.Bookings
            .Include(b => b.Car)
            .Where(b => b.UserId == user!.Id)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
        return View(bookings);
    }
}
