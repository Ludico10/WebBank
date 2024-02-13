using WebBank.AppCore.Entities;

namespace WebBank.AppCore.Interfaces;

public interface IFamilyService
{
    public Task<List<FamilyStatus>> GetAll();
}
