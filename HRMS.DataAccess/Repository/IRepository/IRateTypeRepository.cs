using HRMS.DataAccess.Repository.IRepository;
using HRMS.Models.Entities;

namespace HRMS.DataAccess.Repository
{
    public interface IRateTypeRepository : IRepository<RateType>
    {
        public List<KeyValuePair<int, string>> GetRateTypeListByLang(string culture, string q);

    }
}