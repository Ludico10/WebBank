using WebBank.AppCore.Entities;

namespace WebBank.AppCore.Interfaces;

public interface IDisabilityService
{
    public Task<List<DisabilityGroup>> GetAll();
}
