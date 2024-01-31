namespace WebBank.AppCore.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public DateTime Birthday { get; set; }
        public bool Gender { get; set; }
        public string PassportSeries { get; set; }
        public string PassportNumber { get; set; }
        public string IssuePlace { get; set; }
        public string IssueNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public string IdentificationNumber { get; set; }
        public string BirthPlace { get; set; }
        public Town Town { get; set; }
        public string Address { get; set; }
        public string? HomePhone { get; set; }
        public string? MobilePhone { get; set; }
        public string? Email { get; set; }
        public string? WorkPlace { get; set; }
        public string? WorkPosition { get; set; }
        public Town RegistrationTown { get; set; }
        public string RegistrationAddress { get; set; }
        public FamilyStatus FamilyStatus { get; set; }
        public DisabilityGroup DisabilityGroup { get; set; }
        public bool IsPensioner { get; set; }
        public bool IsConscript { get; set; }
        public int? MonthlyIncome { get; set; }
    }
}
