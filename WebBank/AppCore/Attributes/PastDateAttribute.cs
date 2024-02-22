using System.ComponentModel.DataAnnotations;

namespace WebBank.AppCore.Attributes;

public class PastDateAttribute : ValidationAttribute
{
    public override bool IsValid(object? value) => value is DateOnly date && date <= DateOnly.FromDateTime(DateTime.Now);
}
