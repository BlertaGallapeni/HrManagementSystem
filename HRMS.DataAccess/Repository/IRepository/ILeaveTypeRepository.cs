
using HRMS.Models.Entities;

namespace HRMS.DataAccess.Repository.IRepository
{
    public interface ILeaveTypeRepository : IRepository<LeaveType>
    {
        ICollection<LeaveType> GetEmployeeByLeaveType(int id);
        LeaveType FindById(int id);
        bool isExists(int id);
        bool Create(LeaveType entity);
        bool UpdateLeave(LeaveType entity);
        bool DeleteLeave(LeaveType entity);
        public ICollection<LeaveType> FindAll();

    }
}
