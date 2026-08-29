using CentalMvc.Data;
using CentalMvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CentalMvc.Controllers;

[Authorize(Roles = "Admin")]
public class CarsController : Controller
{
    private readonly AppDbContext _db;
    public CarsController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
        => View(await _db.Cars.OrderByDescending(c => c.Id).ToListAsync());

    [HttpGet]
    public IActionResult Create() => View(new Car());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Car car)
    {
        if (!ModelState.IsValid) return View(car);
        car.CreatedAt = DateTime.UtcNow;
        _db.Cars.Add(car);
        await _db.SaveChangesAsync();
        TempData["Msg"] = "Car added successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var car = await _db.Cars.FindAsync(id);
        if (car == null) return NotFound();
        return View(car);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Car car)
    {
        if (!ModelState.IsValid) return View(car);
        _db.Cars.Update(car);
        await _db.SaveChangesAsync();
        TempData["Msg"] = "Car updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var car = await _db.Cars.FindAsync(id);
        if (car != null)
        {
            _db.Cars.Remove(car);
            await _db.SaveChangesAsync();
            TempData["Msg"] = "Car deleted.";
        }
        return RedirectToAction(nameof(Index));
    }
}
