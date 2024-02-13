using Microsoft.EntityFrameworkCore;
using WebBank.AppCore.Entities;
using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;

namespace WebBank.AppCore.Services;

public class CitizenshipService(MySQLContext context) : ICitizenshipService
{
    public async Task<List<Citizenship>> GetAll() => await context.Citizenships.ToListAsync();
}
