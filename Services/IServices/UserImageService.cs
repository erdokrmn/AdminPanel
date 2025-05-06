using AdminPanel.Services.IServices;

namespace AdminPanel.Services
{
    public class UserImageService : IUserImageService
    {
        private const string DefaultImagePath = "/images/default/user.png";

        public string GetProfileImageUrl(string? imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return DefaultImagePath;

            return imagePath.StartsWith("/") ? imagePath : "/" + imagePath;
        }
    }
}
