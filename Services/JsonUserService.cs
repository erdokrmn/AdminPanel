using AdminPanel.Models;
using AdminPanel.Services.IServices;
using System.Text.Json;

namespace AdminPanel.Services
{
    public class JsonUserService :IUserService
    {
        private readonly List<User> _users;

        public JsonUserService()
        {
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "users.json");
            var json = File.ReadAllText(jsonPath);
            _users = JsonSerializer.Deserialize<List<User>>(json) ?? new();
        }

        public LoginResult ValidateUser(string username, string password)
        {
            username = username?.Trim();
            password = password?.Trim();
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return new LoginResult { Success = false, ErrorMessage = "Kullanıcı adı ve şifre boş olamaz." };

            foreach (var u in _users)
            {
                Console.WriteLine($"DB: '{u.UserName}' | Input: '{username}' | Equal: {u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)}");
            }

            var user = _users.FirstOrDefault(u =>
                !string.IsNullOrWhiteSpace(u.UserName) &&
                u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user == null)
                return new LoginResult { Success = false, ErrorMessage = "Kullanıcı bulunamadı." };

            if (user.PasswordHash != password)
                return new LoginResult { Success = false, ErrorMessage = "Şifre hatalı." };

            return new LoginResult { Success = true, User = user };
        }

    }
}
