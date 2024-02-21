using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebBank.AppCore.Entities;
using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;

namespace WebBank.Pages
{
	public class DepositContractModel(MySQLContext context, IDepositService depositService) : PageModel
	{
		private readonly IDepositService _depositService = depositService;
		private readonly MySQLContext _context = context;

        [BindProperty(SupportsGet = true)]
        public Client? CurrentClient { get; set; }
		[BindProperty]
		public string ContractName { get; set; } = string.Empty;
		[BindProperty(SupportsGet = true)]
		public double StartPayment { get; set; } = 0;
		[BindProperty(SupportsGet = true)]
		public DepositProgram? ChoosenProgram { get; set; }
		public List<DepositProgram> DepositPrograms { get; set; } = [];

        public IActionResult OnGet(int clientId)
		{
			CurrentClient = _context.Clients.FirstOrDefault(c => c.Id == clientId);
			if (CurrentClient == null || !CurrentClient.IsActive)
			{
				return RedirectToPage("Error");
			}

			DepositPrograms = [.. _context.DepositPrograms];
			return Page();
		}

		public IActionResult OnPost()
		{
			if (CurrentClient == null || ChoosenProgram == null)
			{
				return RedirectToPage("Error");
			}

			if (ContractName != null && ContractName != string.Empty)
			{
				_depositService.Create(CurrentClient, ChoosenProgram, Convert.ToInt32(StartPayment * 100), DateTime.Now, ContractName);
			}
			else
			{
				_depositService.Create(CurrentClient, ChoosenProgram, Convert.ToInt32(StartPayment * 100), DateTime.Now);
			}
            return RedirectToPage("Index");
		}
	}
}
