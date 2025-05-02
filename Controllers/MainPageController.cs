using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    public class MainPageController : Controller
    {
        public IActionResult MainPage()
        {
            return View();
        }
    }
}
