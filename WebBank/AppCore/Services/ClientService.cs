using Microsoft.EntityFrameworkCore;
using WebBank.AppCore.Entities;
using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;

namespace WebBank.AppCore.Services
{
    public class ClientService : IClientService
    {
        public async Task<int> GetClientsCount(MySQLContext context)
        {
            return await context.Clients.Where(c => c.IsActive == true).CountAsync();
        }

        public void ChangeClient(MySQLContext context, Client client)
        {
            context.Update(client);
            context.SaveChangesAsync();
        }

        public void AddClient(MySQLContext context, Client client)
        {
            context.Clients.Add(client);
            context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Client>> GetClientsOnPage(MySQLContext context, int pageNumber, int itemsOnPage)
        {
            return await context.Clients.OrderBy(c => c.Surname)
                                        .Where(c => c.IsActive == true)
                                        .Skip(itemsOnPage * (pageNumber - 1))
                                        .Take(itemsOnPage).ToListAsync();
        }

        public void DeleteClient(MySQLContext context, int id)
        {
            var client = context.Clients.SingleOrDefault(c => c.Id == id);
            if (client != null)
            {
                client.IsActive = false;
                context.SaveChanges();
            }
        }
    }
}
