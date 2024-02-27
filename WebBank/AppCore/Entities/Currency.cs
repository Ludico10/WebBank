namespace WebBank.AppCore.Entities;

public class Currency
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Notation { get; set; }
}
