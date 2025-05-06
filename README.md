# AdminPanel – ASP.NET MVC Admin ve Kullanıcı Paneli

Bu proje, ASP.NET Core MVC teknolojisi kullanılarak geliştirilmiş dinamik bir yönetim panelidir. Giriş/kayıt sistemi, rol bazlı yetkilendirme, admin ve kullanıcıya özel layout yapısı, profil fotoğrafı yönetimi ve modern bir dashboard tasarımı içerir.

## 🚀 Özellikler

- ✅ ASP.NET Identity ile kullanıcı yönetimi
- ✅ Admin ve normal kullanıcı için ayrılmış layout yapısı
- ✅ Kullanıcı kayıt ve giriş işlemleri (Login/Register)
- ✅ Rol bazlı yetkilendirme (Admin/User)
- ✅ Otomatik admin oluşturma (SeedData) 
- ✅ Kullanıcı profil resmi gösterimi (varsayılanla birlikte)
- ✅ Logout, erişim kontrolü ve oturum yönetimi
- ✅ Dark/Light mode desteği (user paneli için) // Gelecek
- ✅ Dashboard kısmı için özel responsive CSS yapısı


## 🔧 Kurulum

1. Bu repoyu klonlayın:
    ```bash
    git clone https://github.com/erdokrmn/AdminPanel.git
    ```

2. Gerekli NuGet paketlerini yükleyin:
    - `Microsoft.AspNetCore.Identity.EntityFrameworkCore`
    - `Microsoft.EntityFrameworkCore.SqlServer`

3. Veritabanını oluşturun:
    ```bash
    Add-Migration InitialCreate
    Update-Database
    ```

4. `SeedData` ile varsayılan admin kullanıcı otomatik oluşturulacaktır:
    ```
    E-posta: admin@example.com
    Şifre: Admin123!
    ```

> **Not:** Şifreyi yayına almadan önce değiştirmeniz önerilir.

## 🖼 Profil Resmi Mantığı

- Kullanıcının profil resmi varsa gösterilir
- Yoksa: `wwwroot/images/default/user.png` kullanılır
- Bu işlem `UserImageService` ile merkezi şekilde yapılır

## 🌐 Giriş Sayfası Tasarımı

- Şık bir HTML/CSS login template entegre edilmiştir
- Light/dark mode toggle özelliği desteklenmektedir

## 🛡 Güvenlik

- ASP.NET Identity altyapısı
- Cookie tabanlı kimlik doğrulama
- `[Authorize]`, `[AllowAnonymous]` ve `[ValidateAntiForgeryToken]` kullanımı
- Rol bazlı erişim ve yönlendirme

## 🧑‍💻 Katkı

Katkıda bulunmak isterseniz, forku alın, geliştirin ve pull request gönderin.

---

© 2025 Erdinç Karaman – Tüm Hakları Saklıdır.


