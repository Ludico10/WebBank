using Microsoft.EntityFrameworkCore;
using WebBank.AppCore.Entities;

namespace WebBank.Infrastructure.Data
{
    public class MySQLContext : DbContext
    {
        public readonly string dbPath = "server=localhost;database=bank_db;user=root;password=";
        public MySQLContext() => Database.EnsureCreated();

        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Town> Towns => Set<Town>();
        public DbSet<Citizenship> Citizenships => Set<Citizenship>();
        public DbSet<ClientCitizenship> ClientCitizenships => Set<ClientCitizenship>();
        public DbSet<DisabilityGroup> DisabilityGroups => Set<DisabilityGroup>();
        public DbSet<FamilyStatus> FamilyStatuses => Set<FamilyStatus>();

        public async Task SaveChangesAsync() => await base.SaveChangesAsync();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseMySQL(dbPath);
    }
}
