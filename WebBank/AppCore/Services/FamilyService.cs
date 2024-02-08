using Microsoft.EntityFrameworkCore;
using WebBank.AppCore.Entities;
using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;

namespace WebBank.AppCore.Services
{
    public class FamilyService : IFamilyService
    {
        public async Task<IEnumerable<FamilyStatus>> GetAllStatuses(MySQLContext context)
        {
            return await context.FamilyStatuses.ToListAsync();
        }
    }
}
