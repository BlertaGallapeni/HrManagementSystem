using HRMS.DataAccess.Repository.IRepository;
using HRMS.Models.Entities;
using HRMS.Models.ViewModels;

namespace HRMS.DataAccess.Repository
{
    public interface IDesignationRepository : IRepository<Designation>
    {
        bool HasEmployee(int id);
        IEnumerable<DesignationVM> GetDesignations();
        public List<KeyValuePair<int, string>> GetDesignationListByLang(string culture, string q);
    }
}
