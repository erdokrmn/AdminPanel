using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// 1. MVC servislerini ekle
builder.Services.AddControllersWithViews();

var app = builder.Build();

// 2. Hata ayýklama için geliþtirme ortamý kontrolü
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// 3. HTTPS ve statik dosya ayarlarý
app.UseHttpsRedirection();
app.UseStaticFiles();

// 4. Routing ve (ileride) Authentication middleware'leri
app.UseRouting();
app.UseAuthentication(); // ileride giriþ sistemi eklediðimizde çalýþacak
app.UseAuthorization();

// 5. Default routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
