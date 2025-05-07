# AdminPanel – ASP.NET MVC Admin ve Kullanıcı Paneli

Bu proje, ASP.NET Core MVC teknolojisi kullanılarak geliştirilmiş dinamik bir yönetim panelidir. Giriş/kayıt sistemi, rol bazlı yetkilendirme, kullanıcı profil yönetimi, aktivite loglama ve responsive admin arayüzü içerir.

---

## 🚀 Özellikler

✅ ASP.NET Identity ile kullanıcı ve rol yönetimi  
✅ Admin ve normal kullanıcı için ayrılmış layout yapısı  
✅ Giriş / Kayıt işlemleri (Login / Register)  
✅ Rol bazlı yetkilendirme (Admin / User)  
✅ Otomatik admin oluşturma (SeedData)  
✅ Kullanıcı profil resmi yönetimi  
✅ Şifre sıfırlama sistemi (e-posta üzerinden)  
✅ Kullanıcı profil güncelleme ekranı (adı, e-posta, görsel)  
✅ Kullanıcı aktivite loglama (veritabanında tutulur) 
✅ Otomatik hata loglama sistemi (exception middleware ile)
   - Oluşan hatalar kullanıcı ve tarayıcı bilgileriyle birlikte veritabanına kaydedilir
   - Admin panelde DataTables tabanlı arayüzle listelenebilir
✅ Aktivite logları admin panelde listelenebilir (DataTables ile)  
✅ Logout, erişim kontrolü ve oturum yönetimi  
✅ Modern responsive dashboard arayüzü  
✅ Light/Dark mode desteği (🌓 Toggle sistemi ile)  
✅ Sidebar aç/kapa özelliği  


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


