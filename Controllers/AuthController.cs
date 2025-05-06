using AdminPanel.Models;
using AdminPanel.Models.ViewModels;
using AdminPanel.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly UserManager<User> _userManager;

        public AuthController(IAuthService authService, UserManager<User> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                // Zaten giriş yapılmış, role göre yönlendir
                if (User.IsInRole("Admin"))
                    return RedirectToAction("Index", "Dashboard");
                else
                    return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _authService.LoginAsync(model.UserName, model.Password, model.RememberMe);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (await _userManager.IsInRoleAsync(user, "Admin"))
                    return RedirectToAction("Index", "Dashboard");

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre");
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _authService.RegisterAsync(model.UserName, model.Email, model.Password);

            if (result.Succeeded)
                return RedirectToAction("Login");

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
