using HRMS.DataAccess.Data;
using HRMS.Models.Entities;
using HRMS.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HRMS.DataAccess.Repository
{
    public class DesignationRepository : Repository<Designation>, IDesignationRepository
    {
        private AppDbContext _db;
        public DesignationRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public bool HasEmployee(int id)
        {
            return _db.Employees.Any(x => x.Designation.Id == id);
        }
        public IEnumerable<DesignationVM> GetDesignations()
        {
            var culture = Thread.CurrentThread.CurrentUICulture.Name;
            var query = _db.Designations.Where(x => x.Deleted != true).Select(x => new DesignationVM
            {
                Id = x.Id,
                Name = culture == "sq-AL" ? x.NameAl : x.Name,
                DepartmentName = culture == "sq-AL" ? x.Department.NameAl : x.Department.Name,
                HasEmployee = _db.Employees.Any(y => y.DesignationId == x.Id)
            });
            return query.ToList();
        }
        public List<KeyValuePair<int, string>> GetDesignationListByLang(string culture, string q)
        {
            if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
            {
                return _db.Designations.Where(x => x.Deleted != true && (culture == "sq-AL" ? x.NameAl.ToLower().StartsWith(q.ToLower()) : x.Name.ToLower().StartsWith(q.ToLower()))).Select(x => new KeyValuePair<int, string>(x.Id, culture == "sq-AL" ? x.NameAl : x.Name)).ToList();
            }
            else
            {
                return _db.Designations.Where(x => x.Deleted != true).Select(x => new KeyValuePair<int, string>(x.Id, culture == "sq-AL" ? x.NameAl : x.Name)).ToList();
            }
        }
    }
}
