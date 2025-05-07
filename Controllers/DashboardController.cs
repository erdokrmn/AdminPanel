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
        public async Task<IActionResult> ErrorLogs()
        {
            var logs = await _context.ErrorLogs
                .OrderByDescending(e => e.Timestamp)
                .Join(
                    _context.Users,
                    error => error.UserId,
                    user => user.Id,
                    (error, user) => new ErrorLogViewModel
                    {
                        Message = error.Message,
                        StackTrace = error.StackTrace,
                        Path = error.Path,
                        UserId = error.UserId,
                        UserName = user.UserName,
                        FullName = user.Name,
                        BrowserInfo = error.BrowserInfo,
                        Timestamp = error.Timestamp
                    }
                )
                .ToListAsync();

            return View(logs);
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
