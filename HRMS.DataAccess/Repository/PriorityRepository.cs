using HRMS.DataAccess.Data;
using HRMS.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.DataAccess.Repository
{
    public class PriorityRepository : Repository<Priority>, IPriorityRepository
    {
        private AppDbContext _db;
        public PriorityRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public List<KeyValuePair<int, string>> GetPriorityListByLang(string culture, string q)
        {
            if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
            {
                return _db.Priorities.Where(x => (culture == "sq-AL" ? x.NameAl.ToLower().StartsWith(q.ToLower()) : x.Name.ToLower().StartsWith(q.ToLower()))).Select(x => new KeyValuePair<int, string>(x.Id, culture == "sq-AL" ? x.NameAl : x.Name)).ToList();
            }
            else
            {
                return _db.Priorities.Select(x => new KeyValuePair<int, string>(x.Id, culture == "sq-AL" ? x.NameAl : x.Name)).ToList();
            }
        }
    }
}
