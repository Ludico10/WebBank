using Microsoft.EntityFrameworkCore;
using WebBank.AppCore.Entities;
using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;

namespace WebBank.AppCore.Services
{
    public class DisabilityService : IDisabilityService
    {
        public async Task<IEnumerable<DisabilityGroup>> GetAllGroups(MySQLContext context)
        {
            return await context.DisabilityGroups.ToListAsync();
        }
    }
}
