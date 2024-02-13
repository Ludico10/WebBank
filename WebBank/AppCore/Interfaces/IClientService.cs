using WebBank.AppCore.Entities;

namespace WebBank.AppCore.Interfaces;

public interface IClientService
{
    public Task<List<Client>> GetPage(int pageNumber, int itemsOnPage);
    public Task<int> CountClients();
    public Task Delete(int id);
    public Task Add(Client client);
    public Task Edit(Client client);
    public Task<Client?> Find(int id);
}
