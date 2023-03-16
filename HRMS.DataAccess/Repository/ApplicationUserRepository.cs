using HRMS.DataAccess.Data;
using HRMS.DataAccess.Repository.IRepository;
using HRMS.Models.Entities;
using HRMS.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private AppDbContext _db;
        public ApplicationUserRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public bool CheckExist(string email, string personalNumber)
        {
            var check = _db.ApplicationUsers.Any(x => x.Email == email || x.PersonalNumber == personalNumber);
            return check;

        }
        public void AddUserThumbnail(UserThumbnail userThumbnail)
        {
            try
            {
                _db.UserThumbnail.Add(userThumbnail);
                _db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool DeleteProfilePicture(string userId)
        {
            try
            {
                var profilePicture = _db.UserThumbnail.FirstOrDefault(p => p.Id == userId);
                if (profilePicture == null)
                {
                    return false;
                }

                _db.UserThumbnail.Remove(profilePicture);
                _db.SaveChanges();

                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DeleteThumbnail(UserThumbnail userThumbnail)
        {
            try
            {
                _db.Remove(userThumbnail);
                _db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetProfilePicturePath(string userId, int thumbnail)
        {
            try
            {
                var upload = _db.UserThumbnail.Where(x => x.Id == userId).FirstOrDefault();
                var path = "";
                if (upload != null)
                {
                    path = upload.Path;
                }

                var final = "";

                if (!string.IsNullOrEmpty(path))
                {
                   // remove ~
                   // var pathwithoutsymbol = path.Substring(1, path.Length - 1);
                    //add_75
                    var splitted = path.Split('.');
                    final = splitted[0] + "_" + thumbnail.ToString() + "." + splitted[1];
                }
                else
                {
                    final = "/images/userProfilePic/notfound.png";
                }
                return final;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public UserThumbnail GetUserThumbail(string userId)
        {
            try
            {
                return _db.UserThumbnail.Where(x => x.Id == userId).FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
