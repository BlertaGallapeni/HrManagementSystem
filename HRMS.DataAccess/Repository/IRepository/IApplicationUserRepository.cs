using HRMS.DataAccess.Repository.IRepository;
using HRMS.Models.Entities;

namespace HRMS.DataAccess.Repository
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        bool CheckExist(string email, string personalNumber);
        UserThumbnail GetUserThumbail(string userId);
        void DeleteThumbnail(UserThumbnail userThumbnail);
        void AddUserThumbnail(UserThumbnail userThumbnail);
        string GetProfilePicturePath(string userId, int thumbnail);
        bool DeleteProfilePicture(string userId);
    }
}