namespace WebBank.AppCore.Entities;

public class SystemInformation
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Value { get; set; }
}
