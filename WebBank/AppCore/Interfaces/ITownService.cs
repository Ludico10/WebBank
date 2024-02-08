using WebBank.AppCore.Entities;
using WebBank.Infrastructure.Data;

namespace WebBank.AppCore.Interfaces
{
    public interface ITownService
    {
        public Task<IEnumerable<Town>> GetAllTowns(MySQLContext context);
    }
}
