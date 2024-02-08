using WebBank.AppCore.Entities;
using WebBank.Infrastructure.Data;

namespace WebBank.AppCore.Interfaces
{
    public interface IClientService
    {
        public Task<IEnumerable<Client>> GetClientsOnPage(MySQLContext context, int pageNumber, int itemsOnPage);
        public Task<int> GetClientsCount(MySQLContext context);
        public void DeleteClient(MySQLContext context, int id);
        public void AddClient(MySQLContext context, Client client);
        public void ChangeClient(MySQLContext context, Client client);
    }
}
