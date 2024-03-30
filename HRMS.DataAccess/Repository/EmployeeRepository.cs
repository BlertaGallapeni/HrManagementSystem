using HRMS.DataAccess.Data;
using HRMS.Models.Entities;
using HRMS.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.DataAccess.Repository
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        private AppDbContext _db;
        public EmployeeRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public bool IsTeamLead(string userId)
        {
            return _db.Projects.Any(x=>x.Team.Employee.ApplicationUser.Id== userId);
        }
        public bool IsTeamLeadOfProject(string userId, int projectId)
        {
            return _db.Projects.Where(x=>x.Id==projectId).Any(x => x.Team.Employee.ApplicationUser.Id == userId);
        }
    }
}
