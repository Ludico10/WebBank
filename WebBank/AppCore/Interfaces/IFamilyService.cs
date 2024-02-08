using WebBank.AppCore.Entities;
using WebBank.Infrastructure.Data;

namespace WebBank.AppCore.Interfaces
{
    public interface IFamilyService
    {
        public Task<IEnumerable<FamilyStatus>> GetAllStatuses(MySQLContext context);
    }
}
