using HRMS.DataAccess.Repository.IRepository;
using HRMS.Models.Entities;
using HRMS.Models.ViewModels;

namespace HRMS.DataAccess.Repository
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        bool HasEmployee(int id);
        IEnumerable<DepartmentVM> GetDepartments();
        List<KeyValuePair<int, string>> GetDepartmentListByLang(string culture, string q);
    }
}