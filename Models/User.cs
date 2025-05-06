using Microsoft.AspNetCore.Identity;

namespace AdminPanel.Models
{
    public class User :IdentityUser
    {
        public string? Name { get; set; }

    }
}
