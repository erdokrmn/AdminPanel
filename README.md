📄 README - AdminPanel ASP.NET Core MVC Projesi
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

👥 Kullanıcı Sistemi
Kullanıcılar wwwroot/data/users.json dosyasından çekilir.

Dosya örneği:
[
  {
    "Id": "GUID",
    "UserName": "admin",
    "PasswordHash": "1234",
    "Role": "Admin"
  },
  {
    "Id": "GUID",
    "UserName": "user",
    "PasswordHash": "1234",
    "Role": "User"
  }
]

🔐 Giriş Sistemi
- Kullanıcı girişinde tüm kontroller JsonUserService içinde yapılır.
- Giriş başarısız olursa: LoginResult modeli ile controller’a detaylı mesaj döner.
- Giriş başarılıysa:
  - ClaimsPrincipal ile cookie oturumu başlatılır
  - Role bilgisine göre yönlendirme yapılır

🚪 Çıkış Sistemi
- Admin panel üst kısmında (topbar) “Çıkış Yap” butonu yer alır
- Buton bir <form> ile AccountController > Logout action'ına POST isteği gönderir
- SignOutAsync() ile oturum temizlenir

🎨 CSS ve Görsel Yapı
Tüm görsel stiller wwwroot/css/dashboard.css içinde yer alır.

Eklenen stiller:
- .logout-button: Kırmızı çıkış butonu
- .topbar-right: Profil ve buton hizalama
- .logout-form: Formu hizalamak için margin sıfırlama
- Responsive yapı @media ile uyarlanmıştır

🧪 Test Edilen Hatalar ve Çözümler
- ValidateUser'da eşleşme sorunu → Trim() ve Equals(..., OrdinalIgnoreCase)
- JSON’dan gelen property eşleşmeme → [JsonPropertyName] ile çözüm
- SignInAsync hata → AddAuthentication() ve UseAuthentication() ile düzeltildi
- Çıkış formu hizalama sorunu → topbar-right içine alınarak çözüldü

📌 Ekstra Bilgiler
- LoginResult modeli, başarılı ve başarısız girişleri ayırt etmenizi sağlar.
- Servis katmanına tüm iş kuralları taşındı, controller yalnızca yönlendirici mantıktadır.
- Proje modüler tasarlandığı için farklı projelerde temel olarak rahatlıkla kullanılabilir.

🗂️ Dosya Yapısı (Kısaca)
/Views
   /Account/Login.cshtml
   /Dashboard/Index.cshtml
   /MainPage/MainPage.cshtml
/Views/Shared
   _Layout.cshtml
   _DashboardLayout.cshtml
/wwwroot/css
   dashboard.css
/wwwroot/data
   users.json

------------------------------------------------------------
Bu yapı iskelet olarak geliştirilmeye uygundur. Geliştirici login yönetimi, yetki seviyesi artırımı, database geçişi gibi işlemleri katmanlı olarak ekleyebilir.