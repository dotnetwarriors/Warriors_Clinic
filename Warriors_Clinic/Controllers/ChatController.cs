using Microsoft.AspNetCore.Mvc;
using Warriors_Clinic.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class ChatController : Controller
{
    private readonly AppDbContext _context;

    public ChatController(AppDbContext context)
    {
        _context = context;
    }

    // 🔵 OPEN CHAT
    public IActionResult Index(int receiverId)
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
        {
            return RedirectToAction("Login", "Account");
        }

        // ✅ LOAD CHAT BETWEEN 2 USERS
        var messages = _context.Messages
            .Where(m =>
                (m.SenderId == userId && m.ReceiverId == receiverId) ||
                (m.SenderId == receiverId && m.ReceiverId == userId))
            .OrderBy(m => m.SentDate)
            .ToList();

        ViewBag.ReceiverId = receiverId;

        return View(messages);
    }

    // 🔵 SEND MESSAGE
    [HttpPost]
    public IActionResult Send(int receiverId, string messageText)
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null || string.IsNullOrEmpty(messageText))
        {
            return RedirectToAction("Index", new { receiverId });
        }

        var msg = new Message
        {
            SenderId = userId,         // ✅ CURRENT USER
            ReceiverId = receiverId,   // ✅ OTHER USER
            MessageText = messageText,
            SentDate = DateTime.Now
        };

        _context.Messages.Add(msg);
        _context.SaveChanges();

        return RedirectToAction("Index", new { receiverId });
    }
}