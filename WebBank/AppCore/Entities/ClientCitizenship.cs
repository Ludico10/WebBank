using Microsoft.EntityFrameworkCore;

namespace WebBank.AppCore.Entities
{
    [Keyless]
    public class ClientCitizenship
    {
        public Client Client { get; set; }
        public Citizenship Citizenship { get; set; }
    }
}
