using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBank.AppCore.Entities;

[PrimaryKey(nameof(ClientId), nameof(CitizenshipId))]
public class ClientCitizenship
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    [ForeignKey(nameof(Client))]
    public int ClientId { get; set; }
    public virtual Client Client { get; set; }

    [ForeignKey(nameof(Citizenship))]
    public int CitizenshipId { get; set; }
    public virtual Citizenship Citizenship { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
