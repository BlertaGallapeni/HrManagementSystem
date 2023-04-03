using AutoMapper;
using HRMS.DataAccess.Data;
using HRMS.DataAccess.Repository.IRepository;
using HRMS.Models.Entities;
using HRMS.Models.ViewModels;
using HRMS.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HRMS.DataAccess.Repository
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        private AppDbContext _db;
        public ProjectRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public bool IsTeamLead(string userId)
        {
            return _db.Projects.Any(x => x.Team.Employee.ApplicationUser.Id == userId);
        }
        public IEnumerable<Project> GetProjectsByRole(string userId, string roleName, string? includeProperties = null)
        {
            IQueryable<Project> query = dbSet;
            if (roleName == SD.Role_Admin)
            {
                query = _db.Projects.Where(x => x.Deleted != true);
            }
            else if (roleName == SD.Role_Employee)
            {
                if (IsTeamLead(userId))
                {
                    query = _db.Projects.Where(x => x.Team.Employee.ApplicationUser.Id == userId || x.Team.TeamMembers.Any(x => x.Employee.ApplicationUser.Id == userId) && x.Deleted != true);
                }
                else
                {
                    query = _db.Projects.Where(x => x.Team.TeamMembers.Any(x => x.Employee.ApplicationUser.Id == userId) && x.Deleted != true);

                }

            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }
    }
}
