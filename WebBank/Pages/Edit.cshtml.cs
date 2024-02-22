using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBank.AppCore.Entities;
using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;

namespace WebBank.Pages
{
    public class EditModel : PageModel
    {
        private readonly IClientService _clientService;
        private MySQLContext _context;

        public EditModel(MySQLContext context, IClientService clientService)
        {
            _clientService = clientService;
            _context = context;

            TownOptions = new SelectList(_context.Towns.AsNoTracking().ToList(), "Id", "Name");
            FamilyStatusOptions = new SelectList(_context.FamilyStatuses.AsNoTracking().ToList(), "Id", "Name");
            DisabilityGroupOptions = new SelectList(_context.DisabilityGroups.AsNoTracking().ToList(), "Id", "Name");
            CitizenshipOptions = new SelectList(_context.Citizenships.ToList(), "Id", "Name");
        }

        [BindProperty]
        public Client Client { get; set; } = new();

        public SelectList TownOptions { get; set; }
        public SelectList FamilyStatusOptions { get; set; }
        public SelectList DisabilityGroupOptions { get; set; }
        public SelectList CitizenshipOptions { get; set; }

        [BindProperty]
        public List<int> CitizenshipIds { get; set; } = [];

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return Page();

            var client = await _clientService.Find(id.Value);
            if (client == null) return NotFound();

            Client = client;
            CitizenshipIds = Client.Citizenships.Select(cc => cc.Citizenship.Id).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (Client == null || CitizenshipIds == null) return Page();

            if (!ModelState.IsValid)
            {
                var message = string.Join("<br />", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                return RedirectToPage("Error", new { message });
            }
            if (Client.IssueDate < Client.Birthday)
            {
                return RedirectToPage("Error", new { message = "Issue date can not be lesser than birthday" });
            }

            Client.Citizenships.Clear();
            Client.Citizenships.AddRange(CitizenshipIds.Select(id => new ClientCitizenship { CitizenshipId = id }));
            try
            {
                if (id == null)
                {
                    await _clientService.Add(Client);
                }
                else
                {
                    await _clientService.Edit(Client);
                }
            }
            catch (DbUpdateException e)
            {
                return RedirectToPage("Error", new
                {
                    message = $"Failed to {(id == null ? "add" : "edit")} entity<br />" + e.InnerException?.Message
                });
            }
            return RedirectToPage("Index");
        }
    }
}
