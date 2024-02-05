using WebBank.AppCore.Entities;
using WebBank.Infrastructure.Data;

namespace WebBank.AppCore.Interfaces
{
    public interface IClientService
    {
        public Task<IEnumerable<Client>> GetClientsOnPage(MySQLContext context, int pageNumber);
    }
}
