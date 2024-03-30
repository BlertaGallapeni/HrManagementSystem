using HRMS.DataAccess.Repository.IRepository;
using HRMS.Models.Entities;
using HRMS.Models.ViewModels;

namespace HRMS.DataAccess.Repository
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        bool IsTeamLead(string userId);
        public bool IsTeamLeadOfProject(string userId, int projectId);

    }
}