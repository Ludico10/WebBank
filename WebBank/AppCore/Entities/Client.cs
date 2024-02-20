using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBank.AppCore.Entities;

public class Client
{

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public int Id { get; set; }

    [MaxLength(50)]
    [RegularExpression(@"^[А-Я][а-я]*$")]
    public string Name { get; set; }

    [MaxLength(50)]
    [RegularExpression(@"^[А-Я][а-я]*$")]
    public string Surname { get; set; }

    [MaxLength(50)]
    [RegularExpression(@"^[А-Я][а-я]*$")]
    public string Patronymic { get; set; }

    //не больше сегодняшней даты - 18 лет
    public DateOnly Birthday { get; set; }

    public Gender Gender { get; set; }

    //серия + номер = альтернативный ключ
    [StringLength(2)]
    [RegularExpression(@"^[A-ZА-Я]{2}$")]
    public string PassportSeries { get; set; }

    //серия + номер = альтернативный ключ
    [StringLength(7)]
    [RegularExpression(@"^\d{7}$")]
    public string PassportNumber { get; set; }

    [MaxLength(300)]
    public string IssuePlace { get; set; }

    //не больше сегодняшней даты
    public DateOnly IssueDate { get; set; }

    //альтернативный ключ
    [StringLength(14)]
    [RegularExpression(@"^\w{14}")]
    public string IdentificationNumber { get; set; }

    [MaxLength(300)]
    public string BirthPlace { get; set; }

    [ForeignKey(nameof(Town))]
    public int TownId { get; set; }

    public virtual Town Town { get; set; }

    [MaxLength(300)]
    public string Address { get; set; }

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

    [ForeignKey(nameof(RegistrationTown))]
    public int RegistrationTownId { get; set; }

    public virtual Town RegistrationTown { get; set; }

    [MaxLength(300)]
    public string RegistrationAddress { get; set; }

    [ForeignKey(nameof(FamilyStatus))]
    public int FamilyStatusId { get; set; }

    public virtual FamilyStatus FamilyStatus { get; set; }

    [ForeignKey(nameof(DisabilityGroup))]
    public int DisabilityGroupId { get; set; }

    public virtual DisabilityGroup DisabilityGroup { get; set; }

    public bool IsPensioner { get; set; }

    public bool IsConscript { get; set; }

    public int? MonthlyIncome { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<ClientCitizenship> Citizenships { get; set; } = [];

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
