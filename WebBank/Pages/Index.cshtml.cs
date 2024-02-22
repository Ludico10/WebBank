using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebBank.AppCore.Entities;
using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;
using static System.Math;

namespace WebBank.Pages
{
    public class IndexModel(IClientService clientService, ITimeService timeService) : PageModel
    {
        private readonly IClientService _clientService = clientService;

        private const int itemsOnPage = 10;

        public DateOnly SystemDate { get; } = timeService.GetSystemDate();
        public List<Client> Clients { get; private set; } = [];
        public int PagesCount { get; private set; } = 1;
        public int PageNumber { get; set; }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _clientService.Delete(id);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnGetAsync(int pageNumber = 1)
        {
            PageNumber = pageNumber;
            var clientsCount = await _clientService.CountClients();
            PagesCount = (int)Max(Ceiling((double)clientsCount / itemsOnPage), 1);

            if (pageNumber <= 0 || pageNumber > PagesCount)
            {
                return NotFound();
            }

            Clients = await _clientService.GetPage(pageNumber, itemsOnPage);
            return Page();
        }
    }
}
