using AdminPanel.Models;

namespace AdminPanel.Services.IServices
{
    public interface IUserService
    {
        LoginResult ValidateUser(string username, string password);

    }
}
