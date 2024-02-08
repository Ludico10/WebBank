using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebBank.AppCore.Entities;
using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;

namespace WebBank.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly MySQLContext _context;
        private readonly IClientService _clientService;

        private const int itemsOnPage = 15;

        public List<Client> Clients { get; private set; } = [];
        public int PagesCount { get; private set; } = 1;

        public IndexModel(MySQLContext context, IClientService clientService, ILogger<IndexModel> logger)
        {
            _logger = logger;
            _context = context;
            _clientService = clientService;
        }

        public IActionResult OnPostDelete(int id)
        {
            _clientService.DeleteClient(_context, id);
            return RedirectToPage();
        }

        public IActionResult OnGet(int pageNumber = 1)
        {
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
