using AdminPanel.Models;
using AdminPanel.Models.Logs;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
    }
}
