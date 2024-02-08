using Microsoft.EntityFrameworkCore;
using WebBank.AppCore.Entities;
using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;

namespace WebBank.AppCore.Services
{
    public class CitizenshipService : ICitizenshipService
    {
        public async Task<IEnumerable<Citizenship>> GetAllCitizenships(MySQLContext context)
        {
            return await context.Citizenships.ToListAsync();
        }
    }
}
