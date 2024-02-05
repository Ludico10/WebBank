using System.ComponentModel.DataAnnotations.Schema;

namespace WebBank.AppCore.Entities
{
    public class Client
    {
        public int Id { get; set; }

        public required string Name { get; set; }
        public required string Surname { get; set; }
        public required string Patronymic { get; set; }
        public required DateTime Birthday { get; set; }
        public required Gender Gender { get; set; }
        public required string PassportSeries { get; set; }
        public required string PassportNumber { get; set; }
        public required string IssuePlace { get; set; }
        public required string IssueNumber { get; set; }
        public required DateTime IssueDate { get; set; }
        public required string IdentificationNumber { get; set; }
        public required string BirthPlace { get; set; }

        [ForeignKey(nameof(Town))]
        public required int TownId { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Town Town { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public required string Address { get; set; }
        public string? HomePhone { get; set; }
        public string? MobilePhone { get; set; }
        public string? Email { get; set; }
        public string? WorkPlace { get; set; }
        public string? WorkPosition { get; set; }

        [ForeignKey(nameof(RegistrationTown))]
        public required int RegistrationTownId { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Town RegistrationTown { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public required string RegistrationAddress { get; set; }

        [ForeignKey(nameof(FamilyStatus))]
        public required int FamilyStatusId { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public FamilyStatus FamilyStatus { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [ForeignKey(nameof(DisabilityGroup))]
        public required int DisabilityGroupId { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public DisabilityGroup DisabilityGroup { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public required bool IsPensioner { get; set; }
        public required bool IsConscript { get; set; }
        public int? MonthlyIncome { get; set; }
        public required bool IsActive { get; set; }
    }
}
