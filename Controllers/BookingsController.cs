using CentalMvc.Data;
using CentalMvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CentalMvc.Controllers;

[Authorize(Roles = "Admin")]
public class BookingsController : Controller
{
    private readonly AppDbContext _db;
    public BookingsController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
        => View(await _db.Bookings.Include(b => b.Car)
                    .OrderByDescending(b => b.CreatedAt).ToListAsync());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, BookingStatus status)
    {
        var b = await _db.Bookings.FindAsync(id);
        if (b != null) { b.Status = status; await _db.SaveChangesAsync(); TempData["Msg"] = "Booking status updated."; }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var b = await _db.Bookings.FindAsync(id);
        if (b != null) { _db.Bookings.Remove(b); await _db.SaveChangesAsync(); TempData["Msg"] = "Booking deleted."; }
        return RedirectToAction(nameof(Index));
    }
}
