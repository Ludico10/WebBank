using Microsoft.EntityFrameworkCore;
using WebBank.AppCore.Entities;
using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;

namespace WebBank.AppCore.Services;

public class DisabilityService(MySQLContext context) : IDisabilityService
{
    public async Task<List<DisabilityGroup>> GetAll() => await context.DisabilityGroups.ToListAsync();
}
