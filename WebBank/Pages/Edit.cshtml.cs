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
        private readonly MySQLContext _context;
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


        public EditModel(MySQLContext context, IClientService clientService, ITownService townService, IFamilyService familyService, IDisabilityService disabilityService, ICitizenshipService citizenshipService)
        {
            _context = context;
            _clientService = clientService;

            Towns = new SelectList(townService.GetAll().Result, "Id", "Name");
            FamilyStatuses = new SelectList(familyService.GetAll().Result, "Id", "Name");
            DisabilityGroups = new SelectList(disabilityService.GetAll().Result, "Id", "Name");
            Citizenships = citizenshipService.GetAll().Result.ToList();
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
