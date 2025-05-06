using AdminPanel.Models;
using AdminPanel.Models.ViewModels;
using AdminPanel.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly Services.IServices.IEmailSender _emailSender;
        private readonly UserManager<User> _userManager;

        public AuthController(IAuthService authService, UserManager<User> userManager, Services.IServices.IEmailSender emailSender)
        {
            _authService = authService;
            _emailSender = emailSender;
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
        public async Task<IActionResult> EmailConfirmationPending(string email)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                TempData["SuccessMessage"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("Login");
            }

            ViewBag.Email = email;
            ViewBag.IsConfirmed = user.EmailConfirmed; // ✅ JavaScript ile kontrol için
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CheckEmailConfirmed(string email)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return Json(new { success = false });

            return Json(new { success = true, confirmed = user.EmailConfirmed });
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
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var confirmLink = Url.Action("ConfirmEmail", "Auth",
                    new { userId = user.Id, token = token },
                protocol: HttpContext.Request.Scheme);

                await _emailSender.SendEmailAsync(user.Email, "E-Posta Doğrulama",
                    $"Hesabınızı doğrulamak için <a href='{confirmLink}'>buraya tıklayın</a>");

                return RedirectToAction("EmailConfirmationPending", new { email = user.Email });

            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResendConfirmation(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || user.EmailConfirmed)
            {
                TempData["SuccessMessage"] = "E-posta zaten doğrulanmış. Lütfen giriş yapın.";
                return RedirectToAction("Login");
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmLink = Url.Action("ConfirmEmail", "Auth",
                new { userId = user.Id, token = token },
                protocol: HttpContext.Request.Scheme);

            await _emailSender.SendEmailAsync(user.Email, "Yeniden E-Posta Doğrulama",
                $"Hesabınızı doğrulamak için <a href='{confirmLink}'>buraya tıklayın</a>");

            TempData["Resent"] = true;
            return RedirectToAction("EmailConfirmationPending", new { email });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "E-posta başarıyla doğrulandı. Giriş yapabilirsiniz.";
                return RedirectToAction("Login", "Auth");
            }

            return View("Error");
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
