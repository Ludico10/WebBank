using Microsoft.EntityFrameworkCore;
using WebBank.AppCore.Entities;
using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;

namespace WebBank.AppCore.Services;

public class TownService(MySQLContext context) : ITownService
{
    public async Task<List<Town>> GetAll() => await context.Towns.ToListAsync();
}
