using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBank.AppCore.Entities
{
    [PrimaryKey(nameof(ClientId), nameof(CitizenshipId))]
    public class ClientCitizenship
    {
        [ForeignKey(nameof(Client))]
        public required int ClientId { get; set; }
        public Client Client { get; set; }

        [ForeignKey(nameof(Citizenship))]
        public required int CitizenshipId { get; set; }
        public Citizenship Citizenship { get; set; }
    }
}
