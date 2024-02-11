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

            Towns = new SelectList(townService.GetAllTowns(_context).Result, "Id", "Name");
            FamilyStatuses = new SelectList(familyService.GetAllStatuses(_context).Result, "Id", "Name");
            DisabilityGroups = new SelectList(disabilityService.GetAllGroups(_context).Result, "Id", "Name");
            Citizenships = citizenshipService.GetAllCitizenships(_context).Result.ToList();
        }

        public void OnGet(int? id)
        {
            if (id != null)
            {
                Client = _clientService.GetClientById(_context, id.Value).Result;
                if (Client != null)
                {
                    BirthPlace = Client.BirthPlace;
                }
            }
        }

        public IActionResult OnPost(int? id)
        {
            if (Client != null)
            {
                Client.BirthPlace = BirthPlace;
                if (id != null)
                {
                    _clientService.ChangeClient(_context, Client);
                }
                else
                {
                    _clientService.AddClient(_context, Client);
                }

                return RedirectToPage("Index");
            }

            return Page();
        }
    }
}
