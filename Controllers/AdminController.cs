using CentalMvc.Data;
using CentalMvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CentalMvc.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly AppDbContext _db;
    public AdminController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        ViewBag.CarCount = await _db.Cars.CountAsync();
        ViewBag.AvailableCars = await _db.Cars.CountAsync(c => c.IsAvailable);
        ViewBag.BookingCount = await _db.Bookings.CountAsync();
        ViewBag.PendingBookings = await _db.Bookings.CountAsync(b => b.Status == BookingStatus.Pending);
        ViewBag.UserCount = await _db.Users.CountAsync();
        ViewBag.Revenue = await _db.Bookings
            .Where(b => b.Status == BookingStatus.Confirmed || b.Status == BookingStatus.Completed)
            .SumAsync(b => (decimal?)b.TotalPrice) ?? 0m;

        ViewBag.RecentBookings = await _db.Bookings
            .Include(b => b.Car)
            .OrderByDescending(b => b.CreatedAt)
            .Take(5)
            .ToListAsync();

        return View();
    }
}
