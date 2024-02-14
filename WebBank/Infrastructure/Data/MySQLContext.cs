using Microsoft.EntityFrameworkCore;
using WebBank.AppCore.Entities;

namespace WebBank.Infrastructure.Data;

public class MySQLContext : DbContext
{
    public readonly string dbPath = "server=localhost;database=bank_db;user=root;password=";

    public MySQLContext()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Town> Towns => Set<Town>();
    public DbSet<Citizenship> Citizenships => Set<Citizenship>();
    public DbSet<DisabilityGroup> DisabilityGroups => Set<DisabilityGroup>();
    public DbSet<FamilyStatus> FamilyStatuses => Set<FamilyStatus>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseMySQL(dbPath)
            .UseLazyLoadingProxies();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>().HasAlternateKey(c => c.IdentificationNumber);
        modelBuilder.Entity<Client>().HasAlternateKey(c => new { c.PassportSeries, c.PassportNumber });

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
            new() { Id = 2, Name = "1 группа" },
            new() { Id = 3, Name = "2 группа" },
            new() { Id = 4, Name = "3 группа" },
        ]);

        modelBuilder.Entity<FamilyStatus>().HasData(
        [
            new() { Id = 1, Name = "Не женат/замужем" },
            new() { Id = 2, Name = "Состоит в браке" }
        ]);

        modelBuilder.Entity<Citizenship>().HasData(
        [
            new() { Id = 1, Name = "Республика Беларусь" },
            new() { Id = 2, Name = "Российская Федерация" }
        ]);

        modelBuilder.Entity<Client>().HasData(
        [
            new
            {
                Id = 1,
                Name = "Иван",
                Surname = "Иванов",
                Patronymic = "Иванович",
                Birthday = new DateTime(2000, 1, 1),
                BirthPlace = "Республика Беларусь, Могилёвская обл, д. Горы, дом 71",
                Gender = Gender.Male,
                PassportSeries = "AB",
                PassportNumber = "1234567",
                IssuePlace = "Второоктябрьская 15",
                IssueDate = new DateTime(2002, 2, 2),
                IdentificationNumber = "1234567C123PB1",
                TownId = 1,
                Address = "Первомайская 44",
                RegistrationTownId = 1,
                RegistrationAddress = "Третьефевральская 33",
                FamilyStatusId = 1,
                DisabilityGroupId = 1,
                IsPensioner = false,
                IsConscript = false,
                IsActive = true
            },
            new
            {
                Id = 2,
                Name = "Фёдор",
                Surname = "Федоровев",
                Patronymic = "Фёдорович",
                Birthday = new DateTime(2000, 2, 2),
                BirthPlace = "Республика Беларусь, Брестская обл, д. Горки, дом 72",
                Gender = Gender.Male,
                PassportSeries = "AС",
                PassportNumber = "1224567",
                IssuePlace = "Второоктябрьская 15",
                IssueDate = new DateTime(2002, 3, 23),
                IdentificationNumber = "2234567C123PB2",
                TownId = 2,
                Address = "Второапрельская 44",
                RegistrationTownId = 4,
                RegistrationAddress = "Третьефевральская 33",
                FamilyStatusId = 2,
                DisabilityGroupId = 3,
                IsPensioner = true,
                IsConscript = true,
                IsActive = true
            },
            new
            {
                Id = 3,
                Name = "Сергей",
                Surname = "Павловец",
                Patronymic = "Валерьевич",
                Birthday = new DateTime(2003, 7, 25),
                BirthPlace = "Республика Беларусь, Брест",
                Gender = Gender.Male,
                PassportSeries = "AС",
                PassportNumber = "1234568",
                IssuePlace = "Второоктябрьская 16",
                IssueDate = new DateTime(2005, 3, 23),
                IdentificationNumber = "2234567C123PB2",
                TownId = 3,
                Address = "Второапрельская 14",
                RegistrationTownId = 3,
                RegistrationAddress = "Третьефевральская 3",
                FamilyStatusId = 1,
                DisabilityGroupId = 2,
                IsPensioner = false,
                IsConscript = true,
                IsActive = true
            },
            new
            {
                Id = 4,
                Name = "Казимирка",
                Surname = "Ктотоев",
                Patronymic = "Ктотович",
                Birthday = new DateTime(2004, 7, 25),
                BirthPlace = "Республика Беларусь, Брест",
                Gender = Gender.Female,
                PassportSeries = "AС",
                PassportNumber = "1234569",
                IssuePlace = "Второоктябрьская 16",
                IssueDate = new DateTime(2005, 3, 24),
                IdentificationNumber = "4234567C123PB3",
                TownId = 3,
                Address = "Второапрельская 14",
                RegistrationTownId = 4,
                RegistrationAddress = "Третьефевральская 3",
                FamilyStatusId = 2,
                DisabilityGroupId = 3,
                IsPensioner = true,
                IsConscript = false,
                IsActive = true
            },
            new
            {
                Id = 5,
                Name = "Владислава",
                Surname = "Ктотоев",
                Patronymic = "Ктотович",
                Birthday = new DateTime(2004, 7, 25),
                BirthPlace = "Республика Беларусь, Брест",
                Gender = Gender.Female,
                PassportSeries = "AС",
                PassportNumber = "1234569",
                IssuePlace = "Второоктябрьская 16",
                IssueDate = new DateTime(2005, 3, 24),
                IdentificationNumber = "4234767C123PB3",
                TownId = 4,
                Address = "Второапрельская 14",
                RegistrationTownId = 4,
                RegistrationAddress = "Третьефевральская 3",
                FamilyStatusId = 2,
                DisabilityGroupId = 3,
                IsPensioner = true,
                IsConscript = false,
                IsActive = true
            },
            new
            {
                Id = 6,
                Name = "Дарья",
                Surname = "Ктотоев",
                Patronymic = "Ктотович",
                Birthday = new DateTime(2005, 7, 25),
                BirthPlace = "Республика Беларусь, Брест",
                Gender = Gender.Female,
                PassportSeries = "AС",
                PassportNumber = "1234569",
                IssuePlace = "Второоктябрьская 16",
                IssueDate = new DateTime(2006, 3, 24),
                IdentificationNumber = "4234787C123PB3",
                TownId = 4,
                Address = "Второапрельская 14",
                RegistrationTownId = 4,
                RegistrationAddress = "Третьефевральская 3",
                FamilyStatusId = 2,
                DisabilityGroupId = 3,
                IsPensioner = true,
                IsConscript = false,
                IsActive = true
            },
            new
            {
                Id = 7,
                Name = "Дамир",
                Surname = "Ктотоев",
                Patronymic = "Ктотович",
                Birthday = new DateTime(2005, 7, 25),
                BirthPlace = "Республика Беларусь, Брест",
                Gender = Gender.Male,
                PassportSeries = "AС",
                PassportNumber = "1234569",
                IssuePlace = "Второоктябрьская 16",
                IssueDate = new DateTime(2006, 3, 24),
                IdentificationNumber = "2224787C123PB3",
                TownId = 4,
                Address = "Второапрельская 14",
                RegistrationTownId = 4,
                RegistrationAddress = "Третьефевральская 3",
                FamilyStatusId = 2,
                DisabilityGroupId = 3,
                IsPensioner = true,
                IsConscript = false,
                IsActive = true
            },
            new
            {
                Id = 8,
                Name = "Дмитрий",
                Surname = "Ктотоев",
                Patronymic = "Ктотович",
                Birthday = new DateTime(2005, 7, 25),
                BirthPlace = "Республика Беларусь, Брест",
                Gender = Gender.Male,
                PassportSeries = "AС",
                PassportNumber = "1234569",
                IssuePlace = "Второоктябрьская 16",
                IssueDate = new DateTime(2006, 3, 24),
                IdentificationNumber = "2124787C123PB3",
                TownId = 4,
                Address = "Второапрельская 14",
                RegistrationTownId = 4,
                RegistrationAddress = "Третьефевральская 3",
                FamilyStatusId = 2,
                DisabilityGroupId = 3,
                IsPensioner = true,
                IsConscript = false,
                IsActive = true
            },
            new
            {
                Id = 9,
                Name = "Фёдор",
                Surname = "Ктотоев",
                Patronymic = "Ктотович",
                Birthday = new DateTime(2005, 7, 25),
                BirthPlace = "Республика Беларусь, Брест",
                Gender = Gender.Male,
                PassportSeries = "AС",
                PassportNumber = "1234569",
                IssuePlace = "Второоктябрьская 16",
                IssueDate = new DateTime(2006, 3, 24),
                IdentificationNumber = "212478C123PB3",
                TownId = 4,
                Address = "Второапрельская 14",
                RegistrationTownId = 4,
                RegistrationAddress = "Третьефевральская 3",
                FamilyStatusId = 2,
                DisabilityGroupId = 3,
                IsPensioner = true,
                IsConscript = false,
                IsActive = true
            },
        ]);

        modelBuilder.Entity<Client>()
            .HasMany(e => e.Citizenships)
            .WithMany(e => e.Clients)
            .UsingEntity(j => j.HasData(
                [
                    new { ClientsId = 1, CitizenshipsId = 1 },
                    new { ClientsId = 2, CitizenshipsId = 1 },
                    new { ClientsId = 2, CitizenshipsId = 2 },
                    new { ClientsId = 3, CitizenshipsId = 1 },
                    new { ClientsId = 4, CitizenshipsId = 1 },
                    new { ClientsId = 5, CitizenshipsId = 1 },
                    new { ClientsId = 6, CitizenshipsId = 1 },
                    new { ClientsId = 7, CitizenshipsId = 1 },
                    new { ClientsId = 8, CitizenshipsId = 1 },
                    new { ClientsId = 9, CitizenshipsId = 1 }
                ]));
    }
}
