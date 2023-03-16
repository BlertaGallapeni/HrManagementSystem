using HRMS.DataAccess.Data;
using HRMS.Models.Entities;
using HRMS.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.DataAccess.Repository
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        private AppDbContext _db;
        public ClientRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

 
    }
}
