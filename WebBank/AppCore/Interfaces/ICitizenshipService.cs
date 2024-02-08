using WebBank.AppCore.Entities;
using WebBank.Infrastructure.Data;

namespace WebBank.AppCore.Interfaces
{
    public interface ICitizenshipService
    {
        public Task<IEnumerable<Citizenship>> GetAllCitizenships(MySQLContext context);
    }
}
