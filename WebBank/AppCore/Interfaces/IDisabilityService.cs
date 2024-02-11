using WebBank.AppCore.Entities;
using WebBank.Infrastructure.Data;

namespace WebBank.AppCore.Interfaces
{
    public interface IDisabilityService
    {
        public Task<IEnumerable<DisabilityGroup>> GetAllGroups(MySQLContext context);
    }
}
