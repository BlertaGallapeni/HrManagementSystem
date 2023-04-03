using HRMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.DataAccess.Repository.IRepository
{
    public interface ILeaveRepository : IRepository<Leave>
    {
        bool CheckAllocation(int leavetypeid, string employeeid);
        ICollection<Leave> GetLeavesByEmployee(string employeeid);
        Leave GetLeavesByEmployeeAndType(string employeeid, int leavetypeid);
        Leave FindById(int id);
        bool isExists(int id);
        bool Create(Leave entity);
        bool UpdateLeave(Leave entity);
        bool DeleteLeave(Leave entity);
    }
}
