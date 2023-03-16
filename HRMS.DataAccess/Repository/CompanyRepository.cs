using HRMS.DataAccess.Data;
using HRMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.DataAccess.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private AppDbContext _db;
        public CompanyRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
