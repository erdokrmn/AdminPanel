using AdminPanel.Data;
using AdminPanel.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ActivityLogs()
        {
            var logs = await _context.ActivityLogs
                .OrderByDescending(x => x.Timestamp)
                .Join(
                    _context.Users,
                    log => log.UserId,
                    user => user.Id,
                    (log, user) => new ActivityLogViewModel
                    {
                        UserName = user.UserName,
                        FullName=user.Name,
                        Action = log.Action,
                        Timestamp = log.Timestamp,
                        IpAddress = log.IpAddress,
                        BrowserInfo = log.BrowserInfo
                    }
                )
                .ToListAsync();

            return View(logs);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
