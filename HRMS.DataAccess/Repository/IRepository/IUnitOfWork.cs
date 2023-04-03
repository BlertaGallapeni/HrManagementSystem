using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IApplicationUserRepository ApplicationUser { get; }
        ICompanyRepository Company { get; }
        IDepartmentRepository Department { get; }
        IEmployeeRepository Employee { get; }
        IDesignationRepository Designation { get; }
        IClientRepository Client { get; }
        IStatusRepository Status { get; }
        IProjectRepository Project { get; }
        ITeamRepository Team { get; }
        IPriorityRepository Priority { get; }
        IRateTypeRepository RateType { get; }
        ITeamMemberRepository TeamMember { get; }
        ITaskRepository Task { get; }
        ILeaveRepository Leave { get; }
        ILeaveRequestRepository LeaveRequest { get; }
        ILeaveTypeRepository LeaveType { get; }

        void Save();
    }
}
