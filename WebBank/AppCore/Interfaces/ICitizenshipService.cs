using WebBank.AppCore.Entities;

namespace WebBank.AppCore.Interfaces;

public interface ICitizenshipService
{
    public Task<List<Citizenship>> GetAll();
}
