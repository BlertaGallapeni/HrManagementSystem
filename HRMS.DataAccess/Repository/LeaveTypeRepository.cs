using HRMS.DataAccess.Data;
using HRMS.DataAccess.Repository.IRepository;
using HRMS.Models.Entities;

namespace HRMS.DataAccess.Repository
{
    public class LeaveTypeRepository : Repository<LeaveType>, ILeaveTypeRepository
    {
        private readonly AppDbContext _db;

        public LeaveTypeRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public ICollection<LeaveType> FindAll()
        {
            var leaveTypes = _db.LeaveTypes.ToList();
            return leaveTypes;
        }
        public LeaveType FindById(int id)
        {
            var leaveType = _db.LeaveTypes.Find(id);
            return leaveType;
        }

        public ICollection<LeaveType> GetEmployeeByLeaveType(int id)
        {
            throw new NotImplementedException();
        }
        public bool Create(LeaveType entity)
        {
            _db.LeaveTypes.Add(entity);
            return Save();
        }
        
        public bool DeleteLeave(LeaveType entity)
        {
            _db.LeaveTypes.Remove(entity);
            return Save();
        }
        public bool isExists(int id)
        {
            var exist = _db.LeaveTypes.Any(q => q.Id == id);
            return exist;
        }
        public bool UpdateLeave(LeaveType entity)
        {
            _db.LeaveTypes.Update(entity);
            return Save();
        }
        public bool Save()
        {
            var changes = _db.SaveChanges();
            return changes > 0;
        }

    }
}
