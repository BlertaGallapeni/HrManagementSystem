using HRMS.DataAccess.Data;
using HRMS.DataAccess.Repository.IRepository;
using HRMS.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HRMS.DataAccess.Repository
{
    public class LeaveRepository : Repository<Leave>, ILeaveRepository
    {
        private readonly AppDbContext _db;

        public LeaveRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public bool CheckAllocation(int leavetypeid, string employeeid)
        {
            var period = DateTime.Now.Year;
            return FindAll()
                .Where(q => q.Employee.ApplicationUserId == employeeid && q.LeaveTypeId == leavetypeid && q.Period == period)
                .Any();
        }
        public bool Create(Leave entity)
        {
            _db.Leaves.Add(entity);
            return Save();
        }

        public bool DeleteLeave(Leave entity)
        {
            _db.Leaves.Remove(entity);
            return Save();
        }
        public ICollection<Leave> FindAll()
        {
            var LeaveAllocations = _db.Leaves
                .Include(q => q.LeaveType)
                .Include(q => q.Employee)
                .ToList();
            return LeaveAllocations;
        }

        public Leave FindById(int id)
        {
            var LeaveAllocation = _db.Leaves
                .Include(q => q.LeaveType)
                .Include(q => q.Employee)
                .FirstOrDefault(q => q.Id == id);
            return LeaveAllocation;
        }

        public ICollection<Leave> GetLeavesByEmployee(string employeeid)
        {
            var period = DateTime.Now.Year;
            return FindAll()
                .Where(q => q.Employee.ApplicationUserId == employeeid && q.Period == period)
                .ToList();
        }

        public Leave GetLeavesByEmployeeAndType(string employeeid, int leavetypeid)
        {
            var period = DateTime.Now.Year;
            return FindAll()
                .FirstOrDefault(q => q.Employee.ApplicationUserId == employeeid && q.Period == period && q.LeaveTypeId == leavetypeid);
        }

        public bool isExists(int id)
        {
            var exist = _db.Leaves.Any(q => q.Id == id);
            return exist;
        }
        public bool Save()
        {
            var changes = _db.SaveChanges();
            return changes > 0;
        }

        public bool UpdateLeave(Leave entity)
        {
            _db.Leaves.Update(entity);
            return Save();
        }

    }
}
