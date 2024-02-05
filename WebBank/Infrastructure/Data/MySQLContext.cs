using Microsoft.EntityFrameworkCore;
using WebBank.AppCore.Entities;

namespace WebBank.Infrastructure.Data
{
    public class MySQLContext : DbContext
    {
        public readonly string dbPath = "server=localhost;database=bank_db;user=root;password=Phabletik1044";


        public MySQLContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Town> Towns => Set<Town>();
        public DbSet<Citizenship> Citizenships => Set<Citizenship>();
        public DbSet<ClientCitizenship> ClientCitizenships => Set<ClientCitizenship>();
        public DbSet<DisabilityGroup> DisabilityGroups => Set<DisabilityGroup>();
        public DbSet<FamilyStatus> FamilyStatuses => Set<FamilyStatus>();

        public async Task SaveChangesAsync() => await base.SaveChangesAsync();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseMySQL(dbPath);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Town>().HasData(
            [
                new() { Id = 1, Name = "Минск" },
                new() { Id = 2, Name = "Гродно" },
                new() { Id = 3, Name = "Брест" },
                new() { Id = 4, Name = "Гомель" },
                new() { Id = 5, Name = "Могилев" },
                new() { Id = 6, Name = "Витебск" }
            ]);

            modelBuilder.Entity<DisabilityGroup>().HasData(
            [
                new() { Id = 1, Name = "Здоров" },
                new() { Id = 2, Name = "1-ая группа инвалидности" },
                new() { Id = 3, Name = "2-ая группа инвалидности" },
                new() { Id = 4, Name = "3-ья группа инвалидности" },
            ]);

            modelBuilder.Entity<FamilyStatus>().HasData(
            [
                new() { Id = 1, Name = "Не женат/не замужем" },
                new() { Id = 2, Name = "Состоит в браке" }
            ]);

            modelBuilder.Entity<Citizenship>().HasData(
            [
                new() { Id = 1, Name = "Граждан(-ин/-ка) Республики Беларусь" },
                new() { Id = 2, Name = "Граждан(-ин/-ка) Российской Федерации" }
            ]);

            modelBuilder.Entity<Client>().HasData(
            [
                new() { Id = 1, Name = "Я", Surname = "A", Patronymic = "A", Birthday = DateTime.Today, BirthPlace = " ", Gender = Gender.Male, PassportSeries = "AA", PassportNumber = "1234567", IssuePlace = "A", IssueNumber = "", IssueDate = DateTime.Today, IdentificationNumber = "123456789", TownId = 1, Address = "A", RegistrationTownId = 2, RegistrationAddress = "", FamilyStatusId = 1, DisabilityGroupId = 1, IsPensioner = false, IsConscript = false, IsActive = true },
                new() { Id = 2, Name = "Q", Surname = "F", Patronymic = "O", Birthday = DateTime.Today, BirthPlace = " ", Gender = Gender.Male, PassportSeries = "AA", PassportNumber = "1234567", IssuePlace = "A", IssueNumber = "", IssueDate = DateTime.Today, IdentificationNumber = "123456789", TownId = 1, Address = "A", RegistrationTownId = 2, RegistrationAddress = "", FamilyStatusId = 1, DisabilityGroupId = 1, IsPensioner = false, IsConscript = false, IsActive = true },
                new() { Id = 3, Name = "E", Surname = "G", Patronymic = "B", Birthday = DateTime.Today, BirthPlace = " ", Gender = Gender.Female, PassportSeries = "AA", PassportNumber = "1234567", IssuePlace = "A", IssueNumber = "", IssueDate = DateTime.Today, IdentificationNumber = "123456789", TownId = 1, Address = "A", RegistrationTownId = 2, RegistrationAddress = "", FamilyStatusId = 1, DisabilityGroupId = 1, IsPensioner = false, IsConscript = false, IsActive = true }
            ]);

            modelBuilder.Entity<ClientCitizenship>().HasData(
            [
                new() { ClientId = 1, CitizenshipId = 1 },
                new() { ClientId = 2, CitizenshipId = 1 },
                new() { ClientId = 3, CitizenshipId = 1 },
                new() { ClientId = 3, CitizenshipId = 2 }
            ]);
        }
    }
}
