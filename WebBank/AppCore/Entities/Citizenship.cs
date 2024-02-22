namespace WebBank.AppCore.Entities;

public class Citizenship
{
    public int Id { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string Name { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public virtual ICollection<ClientCitizenship> Clients { get; set; } = [];
}
