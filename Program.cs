using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// 1. MVC servislerini ekle
builder.Services.AddControllersWithViews();

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
