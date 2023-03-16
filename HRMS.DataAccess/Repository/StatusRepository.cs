using HRMS.DataAccess.Data;
using HRMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.DataAccess.Repository
{
    public class StatusRepository : Repository<Status>, IStatusRepository
    {
        private AppDbContext _db;
        public StatusRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
