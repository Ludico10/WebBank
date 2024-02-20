using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebBank.AppCore.Entities;
using WebBank.Infrastructure.Data;

namespace WebBank.Pages
{
	public class DepositContractModel : PageModel
	{
		public Client? CurrentClient { get; set; }
		[BindProperty]
		public string ContractName { get; set; } = string.Empty;
		[BindProperty]
		public double StartPayment { get; set; } = 0;
		[BindProperty]
		public int? ChoosenProgram { get; set; }
		public List<DepositProgram> DepositPrograms { get; set; } = [];

		public IActionResult OnGet(MySQLContext context, int clientId)
		{
			var client = context.Clients.First(c => c.Id == clientId);
			if (client == null || !client.IsActive)
			{
				return RedirectToPage("Error");
			}

			DepositPrograms = [.. context.DepositPrograms];
			return Page();
		}
	}
}
