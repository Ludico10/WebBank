using Microsoft.EntityFrameworkCore;
using WebBank.AppCore.Entities;
using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;

namespace WebBank.AppCore.Services;

public class ClientService(MySQLContext context) : IClientService
{
    public async Task<int> CountClients()
    {
        return await context.Clients.Where(c => c.IsActive == true).CountAsync();
    }

    public async Task<List<Client>> GetPage(int pageNumber, int itemsOnPage)
    {
        var page = await context.Clients
            .OrderBy(c => c.Surname)
            .Where(c => c.IsActive)
            .Skip(itemsOnPage * (pageNumber - 1))
            .Take(itemsOnPage)
            .ToListAsync();

        return page;
    }

    public async Task Delete(int id)
    {
        var client = await context.Clients.SingleOrDefaultAsync(c => c.Id == id);
        if (client == null) return;

        client.IsActive = false;
        await context.SaveChangesAsync();
    }

    public async Task Add(Client client)
    {
        client.IsActive = true;
        context.Clients.Add(client);
        await context.SaveChangesAsync();
    }

    public async Task Edit(Client client)
    {
        client.IsActive = true;
        var entitiesToRemove = context.ClientCitizenships.Where(cc => cc.ClientId == client.Id);
        context.ClientCitizenships.RemoveRange(entitiesToRemove);
        context.Attach(client);
        context.Entry(client).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task<Client?> Find(int id)
    {
        return await context.Clients
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();
    }
}
