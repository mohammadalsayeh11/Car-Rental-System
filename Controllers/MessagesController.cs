using CentalMvc.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CentalMvc.Controllers;

[Authorize(Roles = "Admin")]
public class MessagesController : Controller
{
    private readonly AppDbContext _db;
    public MessagesController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        // mark unread as read when admin opens the list
        var unread = await _db.ContactMessages.Where(m => !m.IsRead).ToListAsync();
        foreach (var m in unread) m.IsRead = true;
        if (unread.Count > 0) await _db.SaveChangesAsync();

        var messages = await _db.ContactMessages
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
        return View(messages);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var m = await _db.ContactMessages.FindAsync(id);
        if (m != null) { _db.ContactMessages.Remove(m); await _db.SaveChangesAsync(); TempData["Msg"] = "Message deleted."; }
        return RedirectToAction(nameof(Index));
    }
}
