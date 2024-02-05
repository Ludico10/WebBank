using Microsoft.EntityFrameworkCore;
using WebBank.AppCore.Entities;
using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;

namespace WebBank.AppCore.Services
{
    public class ClientService : IClientService
    {
        public async Task<IEnumerable<Client>> GetClientsOnPage(MySQLContext context, int pageNumber)
        {
            return await context.Clients.ToListAsync();
        }
    }
}
