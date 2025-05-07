using System.Net;
using System.Security.Claims;
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
        private readonly IActivityLogService _activityLogService;

        public AuthController(IAuthService authService, UserManager<User> userManager, Services.IServices.IEmailSender emailSender, IActivityLogService activityLogService)
        {
            _authService = authService;
            _emailSender = emailSender;
            _userManager = userManager;
            _activityLogService = activityLogService;
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

                // 🔍 Aktivite logu
                await _activityLogService.LogAsync(user.Id, "Kullanıcı giriş yaptı.");
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
            if (!ModelState.IsValid)
                return View(model);

            // 🔒 E-Posta adresi benzersiz mi kontrol et
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Bu e-posta adresi zaten kullanılıyor.");
                return View(model);
            }

            // Kullanıcıyı oluştur
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

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

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
                // 🔍 Aktivite logu
                await _activityLogService.LogAsync(user.Id, "Kullanıcı doğrulama yaptı.");
                TempData["SuccessMessage"] = "E-posta başarıyla doğrulandı. Giriş yapabilirsiniz.";
                return RedirectToAction("Login", "Auth");
            }

            return View("Error");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Güvenlik için hep aynı mesaj
                TempData["Message"] = "Eğer e-posta adresi kayıtlıysa şifre sıfırlama bağlantısı gönderilmiştir.";
                return RedirectToAction("Login");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(token);

            var callbackUrl = Url.Action("ResetPassword", "Auth", new
            {
                userId = user.Id,
                token = encodedToken
            }, protocol: Request.Scheme);

            var emailBody = $"<p>Şifrenizi sıfırlamak için <a href='{callbackUrl}'>buraya tıklayın</a>.</p>";

            await _emailSender.SendEmailAsync(user.Email, "Şifre Sıfırlama", emailBody);

            TempData["Message"] = "Eğer e-posta adresi kayıtlıysa şifre sıfırlama bağlantısı gönderilmiştir.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return BadRequest("Geçersiz istek.");
            }

            var model = new ResetPasswordViewModel
            {
                UserId = userId,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("Login");
            }

            var result = await _userManager.ResetPasswordAsync(user, WebUtility.UrlDecode(model.Token), model.NewPassword);
            if (result.Succeeded)
            {
                // 🔍 Aktivite logu
                await _activityLogService.LogAsync(user.Id, "Kullanıcı şifresini değişti.");
                TempData["SuccessMessage"] = "Şifreniz başarıyla güncellendi.";
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                await _activityLogService.LogAsync(userId, "Oturumu kapattı.");
            }
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
