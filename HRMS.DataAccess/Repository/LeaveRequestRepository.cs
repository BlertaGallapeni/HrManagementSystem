using HRMS.DataAccess.Data;
using HRMS.DataAccess.Repository.IRepository;
using HRMS.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRMS.DataAccess.Repository
{
    public class LeaveRequestRepository : Repository<LeaveRequest>, ILeaveRequestRepository
    {
        private readonly AppDbContext _db;

        public LeaveRequestRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public ICollection<LeaveRequest> FindAll()
        {
            var LeaveHistories = _db.LeaveRequests
                .Include(q => q.RequestingEmployee)
                .Include(q => q.ApprovedBy)
                .Include(q => q.LeaveType)
                .ToList();
            return LeaveHistories;
        }

        public LeaveRequest FindById(int id)
        {
            var LeaveHistory = _db.LeaveRequests
                .Include(q => q.RequestingEmployee)
                .Include(q => q.ApprovedBy)
                .Include(q => q.LeaveType)
                .FirstOrDefault(q => q.Id == id);
            return LeaveHistory;
        }

        public ICollection<LeaveRequest> GetLeaveRequestsByEmployee(string employeeid)
        {
            var leaveRequests = FindAll()
                .Where(q => q.RequestingEmployee.ApplicationUserId == employeeid)
                .ToList();
            return leaveRequests;
        }
        public bool Create(LeaveRequest entity)
        {
            _db.LeaveRequests.Add(entity);
            return Save();
        }

        public bool DeleteLeave(LeaveRequest entity)
        {
            _db.LeaveRequests.Remove(entity);
            return Save();
        }
        public bool isExists(int id)
        {
            var exist = _db.LeaveRequests.Any(q => q.Id == id);
            return exist;
        }
        public bool Save()
        {
            var changes = _db.SaveChanges();
            return changes > 0;
        }

        public bool UpdateLeave(LeaveRequest entity)
        {
            _db.LeaveRequests.Update(entity);
            return Save();
        }

    }
}
