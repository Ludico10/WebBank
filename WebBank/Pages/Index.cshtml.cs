using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebBank.AppCore.Entities;
using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;

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

        public IActionResult OnPostDelete(int id)
        {
            _clientService.DeleteClient(_context, id);
            return RedirectToPage();
        }

        public IActionResult OnGet(int pageNumber = 1)
        {
            PageNumber = pageNumber;
            PagesCount = (int)Math.Ceiling((double)_clientService.GetClientsCount(_context).Result / itemsOnPage);
            if (pageNumber <= PagesCount)
            {
                Clients = _clientService.GetClientsOnPage(_context, pageNumber, itemsOnPage).Result.ToList();
                return Page();
            }

            return RedirectToPage("Error");
        }
    }
}
