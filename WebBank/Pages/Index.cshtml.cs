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

        public List<Client> Clients { get; private set; } = [];

        public IndexModel(MySQLContext context, IClientService clientService, ILogger<IndexModel> logger)
        {
            _logger = logger;
            _context = context;
            _clientService = clientService;
        }

        public void OnGet()
        {
            Clients = _clientService.GetClientsOnPage(_context, 0).Result.ToList();
        }
    }
}
