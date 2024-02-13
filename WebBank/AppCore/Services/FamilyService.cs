using Microsoft.EntityFrameworkCore;
using WebBank.AppCore.Entities;
using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;

namespace WebBank.AppCore.Services;

public class FamilyService(MySQLContext context) : IFamilyService
{
    public async Task<List<FamilyStatus>> GetAll()
    {
        return await context.FamilyStatuses.ToListAsync();
    }
}
