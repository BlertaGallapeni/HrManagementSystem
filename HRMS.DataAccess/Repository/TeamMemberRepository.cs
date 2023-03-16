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
    public class TeamMemberRepository : Repository<TeamMember>, ITeamMemberRepository
    {
        private AppDbContext _db;
        public TeamMemberRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

    }
}
