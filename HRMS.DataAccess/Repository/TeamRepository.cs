using HRMS.DataAccess.Data;
using HRMS.DataAccess.Repository.IRepository;
using HRMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.DataAccess.Repository
{
    public class TeamRepository : Repository<Team>, ITeamRepository
    {
        private AppDbContext _db;
        public TeamRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

    }
}
