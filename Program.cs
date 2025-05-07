using AdminPanel.Data;
using AdminPanel.Models;
using AdminPanel.Services;
using AdminPanel.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. MVC servislerini ekle
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUserImageService, UserImageService>();
builder.Services.AddScoped<IActivityLogService, ActivityLogService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();

// 2. Veritabanı bağlantısı
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Identity servislerini ekle
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// 4. Cookie ayarları
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
});

// 5. AuthService interface'ini ekle
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// 6. Hata ayıklama vs.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// 7. Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await SeedData.Initialize(userManager, roleManager);
}

// 8. Varsayılan rota
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");



app.Run();
