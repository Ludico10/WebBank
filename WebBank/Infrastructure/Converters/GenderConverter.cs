using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebBank.AppCore.Entities;

namespace WebBank.Infrastructure.Converters;

public class GenderConverter : ValueConverter<Gender, string>
{
    public GenderConverter() : base(
        gender => Enum.GetName(typeof(Gender), gender) ?? string.Empty,
        @string => (Gender)Enum.Parse(typeof(Gender), @string))
    { }
}
