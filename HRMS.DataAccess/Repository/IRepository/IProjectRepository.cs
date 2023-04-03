using HRMS.Models.Entities;
using HRMS.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.DataAccess.Repository.IRepository
{
    public interface IProjectRepository : IRepository<Project>
    {
        public IEnumerable<Project> GetProjectsByRole(string userId, string roleName, string? includeProperties = null);
        public bool IsTeamLead(string userId);
    }
}
