using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebBank.AppCore.Entities;
using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;

namespace WebBank.Pages
{
    public class EditModel : PageModel
    {
        private readonly IClientService _clientService;

        [BindProperty]
        public Client? Client { get; set; }
        public SelectList Towns { get; }
        public SelectList FamilyStatuses { get; }
        public SelectList DisabilityGroups { get; }
        public List<Citizenship> Citizenships { get; }

        //костыли
        [BindProperty]
        public string BirthPlace { get; set; } = "";


        public EditModel(MySQLContext context, IClientService clientService)
        {
            _clientService = clientService;

            Towns = new SelectList(context.Towns.ToList(), "Id", "Name");
            FamilyStatuses = new SelectList(context.FamilyStatuses.ToList(), "Id", "Name");
            DisabilityGroups = new SelectList(context.DisabilityGroups.ToList(), "Id", "Name");
            Citizenships = [.. context.Citizenships];
        }

        public IActionResult OnGet(int? id)
        {
            if (id != null)
            {
                Client = _clientService.Find(id.Value).Result;
                if (Client != null)
                {
                    BirthPlace = Client.BirthPlace;
                }
                else
                    return RedirectToPage("Error");
            }
            return Page();
        }

        public IActionResult OnPost(int? id)
        {
            if (Client != null)
            {
                Client.BirthPlace = BirthPlace;
                if (id != null)
                {
                    _clientService.Edit(Client);
                }
                else
                {
                    _clientService.Add(Client);
                }

                return RedirectToPage("Index");
            }

            return Page();
        }
    }
}
