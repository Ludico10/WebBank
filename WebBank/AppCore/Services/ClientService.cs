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
            client.IsActive = true;
            context.Update(client);
            context.SaveChanges();
        }

        public void AddClient(MySQLContext context, Client client)
        {
            client.IsActive = true;
            context.Clients.Add(client);
            context.SaveChanges();
        }

        public async Task<Client?> GetClientById(MySQLContext context, int id)
        {
            return await context.Clients.Where(c => c.Id == id)
                                        .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<Client>> GetClientsOnPage(MySQLContext context, int pageNumber, int itemsOnPage)
        {
            return await context.Clients.OrderBy(c => c.Surname)
                                        .Where(c => c.IsActive)
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
