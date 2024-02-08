using Microsoft.EntityFrameworkCore;
using WebBank.AppCore.Entities;
using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;

namespace WebBank.AppCore.Services
{
    public class TownService : ITownService
    {
        public async Task<IEnumerable<Town>> GetAllTowns(MySQLContext context)
        {
            return await context.Towns.ToListAsync();
        }
    }
}
