📄 README - AdminPanel
------------------------------------------------------------

🔧 Proje Hakkında
Bu proje, ASP.NET Core MVC altyapısı kullanılarak sıfırdan oluşturulmuş, responsive, kullanıcı ve admin panelli bir web uygulamasıdır.
Amaç: Her yeni projeye temel olacak bir admin panel altyapısı oluşturmak.

🧱 Temel Özellikler
- ✅ Admin ve kullanıcı girişi
- ✅ Cookie tabanlı kimlik doğrulama (Authentication)
- ✅ Yetkilere göre yönlendirme (Admin → Dashboard, User → MainPage)
- ✅ JSON dosyasından kullanıcı çekme
- ✅ Gelişmiş hata kontrolü ve ViewBag ile görsel hata bildirimi
- ✅ SweetAlert destekli popup mesaj yapısı (isteğe bağlı)
- ✅ Layout yapısı:
  - _Layout.cshtml → User Panel
  - _DashboardLayout.cshtml → Admin Panel

📌 Ekstra Bilgiler
- LoginResult modeli, başarılı ve başarısız girişleri ayırt etmenizi sağlar.
- Servis katmanına tüm iş kuralları taşındı, controller yalnızca yönlendirici mantıktadır.
- Proje modüler tasarlandığı için farklı projelerde temel olarak rahatlıkla kullanılabilir.

Bu yapı iskelet olarak geliştirilmeye uygundur. Geliştirici login yönetimi, yetki seviyesi artırımı, database geçişi gibi işlemleri katmanlı olarak ekleyebilir.
=======
ASP.NET Core MVC - Genişletilebilir Admin Panel Template

📌 Proje Amacı
==========================

Bu proje, ASP.NET Core MVC ile geliştirilen, hem kullanıcı hem de yönetici paneli içeren, sade, responsive ve yeniden kullanılabilir bir temel projedir.
Amaç, her yeni projeye başlarken tüm login, layout, yönlendirme gibi işlemleri sıfırdan yapmadan, hazır ve düzenli bir altyapı üzerinden hızla ilerlemektir.

🧰 Kullanılan Teknolojiler
==========================

- ASP.NET Core MVC (.NET 6+)
- Razor View Engine
- Cookie Authentication (manual giriş sistemi)
- Responsive tasarım (özel CSS ile)
- İki farklı layout:
    - _Layout.cshtml (Kullanıcı arayüzü)
    - _DashboardLayout.cshtml (Yönetici paneli)

🚀 Kurulum ve Kullanım
==========================

1. Projeyi klonlayın veya kopyalayın:
   git clone [repo-link]

2. Gerekli NuGet paketlerini yükleyin:
   - Visual Studio: Tools > NuGet > Restore
   - CLI: dotnet restore

3. Projeyi başlatın:
   - Visual Studio: F5
   - CLI: dotnet run

🔐 Giriş Bilgisi (Varsayılan)
==========================

Kullanıcı Adı: admin
Şifre: 1234

Not: Bu sabit değerler AccountController içinde örnek olarak tanımlanmıştır. İsterseniz veritabanı bağlantısı ile değiştirebilirsiniz.

🔧 Genişletme Rehberi
==========================

• Role bazlı yetkilendirme → [Authorize(Roles = "...")]
• Dinamik navbar yapısı
• Admin paneline Chart.js vb. grafik kütüphaneleri
• ViewComponent veya PartialView ile modüler yapı
• Otomatik test altyapısı (XUnit, NUnit)

✅ Neden Bu Projede Auth Sistemi Kurulmalı?
==========================

• Her yeni projede giriş/rol kontrolünü tekrar kurmak zaman kaybıdır
• Claim/role bazlı yönlendirme altyapısı çoğu projede ihtiyaçtır
• Giriş sonrası:
    - Admin rolü için → Dashboard yönlendirmesi
    - User rolü için → MainPage yönlendirmesi
• Projeye şimdiden temel Auth servisleri (ClaimsPrincipal + Authorize) eklemek uzun vadede avantaj sağlar

📬 Geliştirici Notu
==========================

Bu proje, sıfırdan admin paneli veya kullanıcı arayüzü tasarlamak yerine bir temel yapı oluşturmak amacıyla geliştirildi.
Layout yapısı, sayfa yönlendirmesi ve stil organizasyonu üzerinden her yeni projeye hızlıca başlanabilir.

🪪 Lisans
==========================

Bu proje kişisel ve ticari projelerde temel olarak kullanılmak üzere geliştirildi. Açık kaynaklı olarak dağıtılabilir.
>>>>>>> 720d0d0580c39c7cdf1808b9e6bffa7c63c2a6c0
