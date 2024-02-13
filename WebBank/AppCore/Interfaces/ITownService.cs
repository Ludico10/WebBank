using WebBank.AppCore.Entities;

namespace WebBank.AppCore.Interfaces;

public interface ITownService
{
    public Task<List<Town>> GetAll();
}
