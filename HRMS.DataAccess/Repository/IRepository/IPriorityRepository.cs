using HRMS.DataAccess.Repository.IRepository;
using HRMS.Models.Entities;

namespace HRMS.DataAccess.Repository
{
    public interface IPriorityRepository : IRepository<Priority>
    {
        public List<KeyValuePair<int, string>> GetPriorityListByLang(string culture, string q);

    }
}