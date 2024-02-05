namespace WebBank.AppCore.Entities;

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
    public required Town Town { get; set; }
    public required string Address { get; set; }
    public string? HomePhone { get; set; }
    public string? MobilePhone { get; set; }
    public string? Email { get; set; }
    public string? WorkPlace { get; set; }
    public string? WorkPosition { get; set; }
    public required Town RegistrationTown { get; set; }
    public required string RegistrationAddress { get; set; }
    public required FamilyStatus FamilyStatus { get; set; }
    public required DisabilityGroup DisabilityGroup { get; set; }
    public required bool IsPensioner { get; set; }
    public required bool IsConscript { get; set; }
    public int? MonthlyIncome { get; set; }
    public required bool IsActive { get; set; }
    public List<Citizenship> Citizenships { get; set; } = [];
}
