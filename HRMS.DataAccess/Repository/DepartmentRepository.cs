using HRMS.DataAccess.Data;
using HRMS.Models.Entities;
using HRMS.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace HRMS.DataAccess.Repository
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        private AppDbContext _db;
        public DepartmentRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public IEnumerable<DepartmentVM> GetDepartments()
        {
            var culture = Thread.CurrentThread.CurrentUICulture.Name;
            var query= _db.Departments.Where(x=>x.Deleted!=true).Select(x=> new DepartmentVM
            {
               Id=x.Id,
               Name =culture=="sq-AL"?x.NameAl:x.Name,
               HasEmployee=_db.Employees.Any(y=>y.DepartmentId==x.Id)
            });
            return query.ToList();
        }
        public bool HasEmployee(int id)
        {
            return _db.Employees.Any(x => x.Department.Id == id);
        }
        public List<KeyValuePair<int, string>> GetDepartmentListByLang(string culture, string q)
        {
            if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
            {
                return _db.Departments.Where(x => x.Deleted!=true && (culture == "sq-AL" ? x.NameAl.ToLower().StartsWith(q.ToLower()) : x.Name.ToLower().StartsWith(q.ToLower()))).Select(x => new KeyValuePair<int, string>(x.Id, culture == "sq-AL" ? x.NameAl : x.Name)).ToList();
            }
            else
            {
                return _db.Departments.Where(x=> x.Deleted!=true).Select(x => new KeyValuePair<int, string>(x.Id, culture == "sq-AL" ? x.NameAl : x.Name)).ToList();
            }
        }

    }
}
