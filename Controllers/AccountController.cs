using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // örnek kontrol – sonra şifreleme ve veri tabanı ile değişecek
            if (username == "admin" && password == "1234")
            {
                // Örnek yönlendirme: Admin paneli
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                // Kullanıcı paneli
                return RedirectToAction("MainPage", "MainPage");
            }
        }
    }
}
