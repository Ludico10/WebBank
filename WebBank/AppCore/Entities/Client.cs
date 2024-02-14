using System.ComponentModel.DataAnnotations;

namespace WebBank.AppCore.Entities;

public class Client
{
    public int Id { get; set; }

    [MaxLength(50)]
    [RegularExpression(@"^[А-Я][а-я]*$")]
    public required string Name { get; set; }

    [MaxLength(50)]
    [RegularExpression(@"^[А-Я][а-я]*$")]
    public required string Surname { get; set; }

    [MaxLength(50)]
    [RegularExpression(@"^[А-Я][а-я]*$")]
    public required string Patronymic { get; set; }

    //не больше сегодняшней даты - 18 лет
    public required DateTime Birthday { get; set; }

    public required Gender Gender { get; set; }

    //серия + номер = альтернативный ключ
    [StringLength(2)]
    [RegularExpression(@"^[A-ZА-Я]{2}$")]
    public required string PassportSeries { get; set; }

    //серия + номер = альтернативный ключ
    [StringLength(7)]
    [RegularExpression(@"^\d{7}$")]
    public required string PassportNumber { get; set; }

    [MaxLength(300)]
    public required string IssuePlace { get; set; }

    //не больше сегодняшней даты
    public required DateTime IssueDate { get; set; }

    //альтернативный ключ
    [StringLength(14)]
    [RegularExpression(@"^\w{14}")]
    public required string IdentificationNumber { get; set; }

    [MaxLength(300)]
    public required string BirthPlace { get; set; }

    public required virtual Town Town { get; set; }

    [MaxLength(300)]
    public required string Address { get; set; }

    [StringLength(7)]
    [RegularExpression(@"^\d{7}$")]
    public string? HomePhone { get; set; }

    [MaxLength(13)]
    [RegularExpression(@"^(\+375|80)\d{9}$")]
    public string? MobilePhone { get; set; }

    [MaxLength(100)]
    [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")] //со стековерфлоу, не проверял, и вообще регэкспами так никто уже не делает, есть класс MailAddress
    public string? Email { get; set; }

    [MaxLength(300)]
    public string? WorkPlace { get; set; }

    [MaxLength(100)]
    public string? WorkPosition { get; set; }

    public required virtual Town RegistrationTown { get; set; }

    [MaxLength(300)]
    public required string RegistrationAddress { get; set; }

    public required virtual FamilyStatus FamilyStatus { get; set; }

    public required virtual DisabilityGroup DisabilityGroup { get; set; }

    public required bool IsPensioner { get; set; }

    public required bool IsConscript { get; set; }

    public int? MonthlyIncome { get; set; }

    public required bool IsActive { get; set; }

    public virtual List<Citizenship> Citizenships { get; set; } = [];
}
