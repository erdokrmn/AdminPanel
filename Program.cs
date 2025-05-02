using AdminPanel.Services.IServices;
using AdminPanel.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// 1. MVC servislerini ekle
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUserService, JsonUserService>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Giri� yap�lmad�ysa y�nlendirme
        options.AccessDeniedPath = "/Account/AccessDenied"; // Yetki yoksa
    });

var app = builder.Build();

// 2. Hata ay�klama i�in geli�tirme ortam� kontrol�
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// 3. HTTPS ve statik dosya ayarlar�
app.UseHttpsRedirection();
app.UseStaticFiles();

// 4. Routing ve (ileride) Authentication middleware'leri
app.UseRouting();
app.UseAuthentication(); // ileride giri� sistemi ekledi�imizde �al��acak
app.UseAuthorization();

// 5. Default routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
