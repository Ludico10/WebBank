using Microsoft.EntityFrameworkCore;
using WebBank.AppCore.Entities;

namespace WebBank.Infrastructure.Data
{
    public class MySQLContext : DbContext
    {
        public readonly string dbPath = "server=localhost;database=bank_db;user=root;password=Phabletik1044";


        public MySQLContext() 
        {
            //Database.EnsureDeleted();
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
            var towns = new List<Town>
            {
                new Town { Id=1, Name="Минск" },
                new Town { Id=2, Name="Гродно" },
                new Town { Id=3, Name="Брест" },
                new Town { Id=4, Name="Гомель" },
                new Town { Id=5, Name="Могилев" },
                new Town { Id=6, Name="Витебск" }
            };
            modelBuilder.Entity<Town>().HasData(towns);

            var disabilityGroups = new List<DisabilityGroup>
            {
                new DisabilityGroup { Id = 1, Name = "Здоров" },
                new DisabilityGroup { Id = 2, Name = "1-ая группа инвалидности" },
                new DisabilityGroup { Id = 3, Name = "2-ая группа инвалидности" },
                new DisabilityGroup { Id = 4, Name = "3-ья группа инвалидности" },
            };
            modelBuilder.Entity<DisabilityGroup>().HasData(disabilityGroups);

            var familyStatuses = new List<FamilyStatus>
            {
                new FamilyStatus { Id = 1, Name = "Не женат/не замужем" },
                new FamilyStatus { Id = 2, Name = "Состоит в браке" }
            };
            modelBuilder.Entity<FamilyStatus>().HasData(familyStatuses);

            var citizenships = new List<Citizenship>
            {
                new Citizenship { Id = 1, Name = "Граждан(-ин/-ка) Республики Беларусь" },
                new Citizenship { Id = 2, Name = "Граждан(-ин/-ка) Российской Федерации" }
            };
            modelBuilder.Entity<Citizenship>().HasData(citizenships);

            var clients = new List<Client>
            {
                new Client { Id = 1, Name = "Я", Surname = "A", Patronymic = "A", Birthday = DateTime.Today, BirthPlace = " ", Gender = true, PassportSeries = "AA", PassportNumber = "1234567", IssuePlace = "A", IssueNumber = "", IssueDate = DateTime.Today, IdentificationNumber = "123456789", Town = towns[0], Address = "A", RegistrationTown = towns[1], FamilyStatus = familyStatuses[0], DisabilityGroup = disabilityGroups[0], IsPensioner = false, IsConscript = false, IsActive = true },
                new Client { Id = 2, Name = "Q", Surname = "F", Patronymic = "O", Birthday = DateTime.Today, BirthPlace = " ", Gender = true, PassportSeries = "AA", PassportNumber = "1234567", IssuePlace = "A", IssueNumber = "", IssueDate = DateTime.Today, IdentificationNumber = "123456789", Town = towns[0], Address = "A", RegistrationTown = towns[1], FamilyStatus = familyStatuses[0], DisabilityGroup = disabilityGroups[0], IsPensioner = false, IsConscript = false, IsActive = true },
                new Client { Id = 3, Name = "E", Surname = "G", Patronymic = "B", Birthday = DateTime.Today, BirthPlace = " ", Gender = true, PassportSeries = "AA", PassportNumber = "1234567", IssuePlace = "A", IssueNumber = "", IssueDate = DateTime.Today, IdentificationNumber = "123456789", Town = towns[0], Address = "A", RegistrationTown = towns[1], FamilyStatus = familyStatuses[0], DisabilityGroup = disabilityGroups[0], IsPensioner = false, IsConscript = false, IsActive = true }
            };
            modelBuilder.Entity<Client>().HasData(clients);

            var clientCitizenships = new List<ClientCitizenship>()
            {
                new ClientCitizenship { Client = clients[0], Citizenship = citizenships[0] },
                new ClientCitizenship { Client = clients[1], Citizenship = citizenships[0] },
                new ClientCitizenship { Client = clients[2], Citizenship = citizenships[0] },
                new ClientCitizenship { Client = clients[2], Citizenship = citizenships[1] }
            };
            modelBuilder.Entity<ClientCitizenship>().HasData(clientCitizenships);
        }
    }
}
