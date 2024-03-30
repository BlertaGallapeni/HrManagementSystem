
using HRMS.Models.Entities;

namespace HRMS.DataAccess.Repository.IRepository
{
    public interface ILeaveRequestRepository : IRepository<LeaveRequest>
    {
        ICollection<LeaveRequest> GetLeaveRequestsByEmployee(string employeeid);
        LeaveRequest FindById(int id);
        bool isExists(int id);
        bool Create(LeaveRequest entity);
        bool UpdateLeave(LeaveRequest entity);
        bool DeleteLeave(LeaveRequest entity);
    }
}
