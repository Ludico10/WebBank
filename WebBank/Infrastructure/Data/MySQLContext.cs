using Microsoft.EntityFrameworkCore;
using WebBank.AppCore.Entities;

namespace WebBank.Infrastructure.Data;

public class MySQLContext : DbContext
{
    public readonly string dbPath = "server=localhost;database=bank_db;user=root;password=Phabletik1044";

    public MySQLContext()
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    public DbSet<SystemInformation> SystemInformation => Set<SystemInformation>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Town> Towns => Set<Town>();
    public DbSet<Citizenship> Citizenships => Set<Citizenship>();
    public DbSet<DisabilityGroup> DisabilityGroups => Set<DisabilityGroup>();
    public DbSet<FamilyStatus> FamilyStatuses => Set<FamilyStatus>();
    public DbSet<Currency> Currencies => Set<Currency>();
    public DbSet<DepositProgram> DepositPrograms => Set<DepositProgram>();
    public DbSet<BankAccount> BankAccounts => Set<BankAccount>();
    public DbSet<ClientDeposit> ClientDeposits => Set<ClientDeposit>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseMySQL(dbPath)
            .UseLazyLoadingProxies();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<DateOnly>()
            .HaveConversion<Converters.DateOnlyConverter>()
            .HaveColumnType("DATE");

        configurationBuilder
            .Properties<Gender>()
            .HaveConversion<Converters.GenderConverter>()
            .HaveColumnType($"ENUM('{nameof(Gender.Male)}', '{nameof(Gender.Female)}')");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Client>()
            .HasAlternateKey(c => c.IdentificationNumber);
        modelBuilder
            .Entity<Client>()
            .HasAlternateKey(c => new { c.PassportSeries, c.PassportNumber });

#if DEBUG
        modelBuilder
            .Entity<SystemInformation>()
            .HasData(
            [
                new() { Id = 1, Name = "TimeDifference", Value = "0" }
            ]);

        modelBuilder
            .Entity<Town>()
            .HasData(
        [
            new() { Id = 1, Name = "Минск" },
            new() { Id = 2, Name = "Гродно" },
            new() { Id = 3, Name = "Брест" },
            new() { Id = 4, Name = "Гомель" },
            new() { Id = 5, Name = "Могилев" },
            new() { Id = 6, Name = "Витебск" }
        ]);

        modelBuilder
            .Entity<DisabilityGroup>()
            .HasData(
        [
            new() { Id = 1, Name = "Здоров" },
            new() { Id = 2, Name = "1 группа" },
            new() { Id = 3, Name = "2 группа" },
            new() { Id = 4, Name = "3 группа" },
        ]);

        modelBuilder
            .Entity<FamilyStatus>()
            .HasData(
        [
            new() { Id = 1, Name = "Не женат/замужем" },
            new() { Id = 2, Name = "Состоит в браке" }
        ]);

        modelBuilder
            .Entity<Citizenship>()
            .HasData(
        [
            new() { Id = 1, Name = "Республика Беларусь" },
            new() { Id = 2, Name = "Российская Федерация" }
        ]);

        modelBuilder
            .Entity<Client>()
            .HasData(
                [
                    new()
                    {
                        Id = 1,
                        Name = "Иван",
                        Surname = "Иванов",
                        Patronymic = "Иванович",
                        Birthday = new DateOnly(2000, 1, 1),
                        BirthPlace = "Республика Беларусь, Могилёвская обл, д. Горы, дом 71",
                        Gender = Gender.Male,
                        PassportSeries = "AB",
                        PassportNumber = "1234567",
                        IssuePlace = "Второоктябрьская 15",
                        IssueDate = new DateOnly(2002, 2, 2),
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
                    new()
                    {
                        Id = 2,
                        Name = "Фёдор",
                        Surname = "Федоровев",
                        Patronymic = "Фёдорович",
                        Birthday = new DateOnly(2000, 2, 2),
                        BirthPlace = "Республика Беларусь, Брестская обл, д. Горки, дом 72",
                        Gender = Gender.Male,
                        PassportSeries = "AС",
                        PassportNumber = "2234567",
                        IssuePlace = "Второоктябрьская 15",
                        IssueDate = new DateOnly(2002, 3, 23),
                        IdentificationNumber = "2234567C123PB1",
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
                    new()
                    {
                        Id = 3,
                        Name = "Сергей",
                        Surname = "Павловец",
                        Patronymic = "Валерьевич",
                        Birthday = new DateOnly(2003, 7, 25),
                        BirthPlace = "Республика Беларусь, Брест",
                        Gender = Gender.Male,
                        PassportSeries = "AС",
                        PassportNumber = "3234567",
                        IssuePlace = "Второоктябрьская 16",
                        IssueDate = new DateOnly(2005, 3, 23),
                        IdentificationNumber = "3234567C123PB1",
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
                    new()
                    {
                        Id = 4,
                        Name = "Казимирка",
                        Surname = "Ктотоев",
                        Patronymic = "Ктотович",
                        Birthday = new DateOnly(2004, 7, 25),
                        BirthPlace = "Республика Беларусь, Брест",
                        Gender = Gender.Female,
                        PassportSeries = "AС",
                        PassportNumber = "4234567",
                        IssuePlace = "Второоктябрьская 16",
                        IssueDate = new DateOnly(2005, 3, 24),
                        IdentificationNumber = "4234567C123PB1",
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
                    new()
                    {
                        Id = 5,
                        Name = "Владислава",
                        Surname = "Ктотоев",
                        Patronymic = "Ктотович",
                        Birthday = new DateOnly(2004, 7, 25),
                        BirthPlace = "Республика Беларусь, Брест",
                        Gender = Gender.Female,
                        PassportSeries = "AС",
                        PassportNumber = "5234567",
                        IssuePlace = "Второоктябрьская 16",
                        IssueDate = new DateOnly(2005, 3, 24),
                        IdentificationNumber = "5234567C123PB1",
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
                    new()
                    {
                        Id = 6,
                        Name = "Дарья",
                        Surname = "Ктотоев",
                        Patronymic = "Ктотович",
                        Birthday = new DateOnly(2005, 7, 25),
                        BirthPlace = "Республика Беларусь, Брест",
                        Gender = Gender.Female,
                        PassportSeries = "AС",
                        PassportNumber = "6234567",
                        IssuePlace = "Второоктябрьская 16",
                        IssueDate = new DateOnly(2006, 3, 24),
                        IdentificationNumber = "6234567C123PB1",
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
                    new()
                    {
                        Id = 7,
                        Name = "Дамир",
                        Surname = "Ктотоев",
                        Patronymic = "Ктотович",
                        Birthday = new DateOnly(2005, 7, 25),
                        BirthPlace = "Республика Беларусь, Брест",
                        Gender = Gender.Male,
                        PassportSeries = "AС",
                        PassportNumber = "7234567",
                        IssuePlace = "Второоктябрьская 16",
                        IssueDate = new DateOnly(2006, 3, 24),
                        IdentificationNumber = "7234567C123PB1",
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
                    new()
                    {
                        Id = 8,
                        Name = "Дмитрий",
                        Surname = "Ктотоев",
                        Patronymic = "Ктотович",
                        Birthday = new DateOnly(2005, 7, 25),
                        BirthPlace = "Республика Беларусь, Брест",
                        Gender = Gender.Male,
                        PassportSeries = "AС",
                        PassportNumber = "8234567",
                        IssuePlace = "Второоктябрьская 16",
                        IssueDate = new DateOnly(2006, 3, 24),
                        IdentificationNumber = "8234567C123PB1",
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
                    new()
                    {
                        Id = 9,
                        Name = "Фёдор",
                        Surname = "Ктотоев",
                        Patronymic = "Ктотович",
                        Birthday = new DateOnly(2005, 7, 25),
                        BirthPlace = "Республика Беларусь, Брест",
                        Gender = Gender.Male,
                        PassportSeries = "AС",
                        PassportNumber = "9234567",
                        IssuePlace = "Второоктябрьская 16",
                        IssueDate = new DateOnly(2006, 3, 24),
                        IdentificationNumber = "9234567C123PB1",
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

        modelBuilder
            .Entity<Client>()
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

        modelBuilder
            .Entity<Currency>()
            .HasData(
            [
                new() { Id = 1, Name = "белорусский рубль", Notation = "BYN" },
                new() { Id = 2, Name = "доллар", Notation = "$" }
            ]);

        modelBuilder
            .Entity<DepositProgram>()
            .HasData(
            [
                new()
                {
                    Id = 1,
                    Name = "1",
                    CurrencyId = 1,
                    Type = DepositType.Revocable,
                    MinimumPayment = 1000,
                    Percent = 30,
                    Period = 365,
                    PercentAccessPeriod = 35
                },
                new()
                {
                    Id = 2,
                    Name = "2",
                    CurrencyId = 1,
                    Type = DepositType.Irrevocable,
                    MinimumPayment = 100,
                    Percent = 50,
                    Period = 35
                }
            ]);

        modelBuilder
            .Entity<BankAccount>()
            .HasData(
            [
                new()
                {
                    Id = 1,
                    Name = "Касса",
                    Number = "0000000000009",
                    CurrencyId = 1,
                    Type = AccountType.Cash,
                },
                new()
                {
                    Id = 2,
                    Name = "Фонд развития",
                    Number = "0001000000009",
                    CurrencyId = 1,
                    Type = AccountType.Fund,
                    Credit = 1000000000
                }
            ]);
#endif
    }
}
