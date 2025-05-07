namespace AdminPanel.Services.IServices
{
    public interface IUserImageService
    {
        Task<string> GetProfileImageUrlAsync();
    }

}
