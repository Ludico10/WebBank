using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebBank.AppCore.Entities;
using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;
using static System.Math;

namespace WebBank.Pages
{
    public class IndexModel(MySQLContext context, IClientService clientService, ILogger<IndexModel> logger) : PageModel
    {
        private readonly ILogger<IndexModel> _logger = logger;
        private readonly MySQLContext _context = context;
        private readonly IClientService _clientService = clientService;

        private const int itemsOnPage = 5;

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
                return RedirectToPage("Error");
            }

            Clients = await _clientService.GetPage(pageNumber, itemsOnPage);
            return Page();
        }
    }
}
