using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebBank.AppCore.Entities;
using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;

namespace WebBank.Pages;

public class CreditProgramModel(
    MySQLContext context,
    ICreditService creditService, 
    ITimeService timeService) : PageModel
{
    public DateOnly SystemDate { get; } = timeService.GetSystemDate();

    [BindProperty]
    public int ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;

    [BindProperty]
    public string ContractName { get; set; } = string.Empty;

    [BindProperty]
    public decimal Amount { get; set; } = 0;

    [BindProperty]
    public int? ChoosenProgram { get; set; }
    public List<CreditProgram> CreditPrograms { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(int clientId)
    {
        ClientId = clientId;
        var client = await context.Clients.FirstOrDefaultAsync(c => c.Id == clientId);
        if (client == null || !client.IsActive)
        {
            return RedirectToPage("Error");
        }
        ClientName = client.Surname + " " + client.Name + " " + client.Patronymic;

        CreditPrograms = [.. context.CreditPrograms];
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ChoosenProgram == null)
        {
            return RedirectToPage("Error");
        }

        var client = context.Clients.FirstOrDefault(c => c.Id == ClientId);
        var program = context.CreditPrograms.FirstOrDefault(c => c.Id == ChoosenProgram.Value);
        if (program == null || client == null)
        {
            return RedirectToPage("Error");
        }

        var sysTime = new DateTime(SystemDate.Year, SystemDate.Month, SystemDate.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        if (ContractName != null && ContractName != string.Empty)
        {
            await creditService.Create(client, program, Amount, sysTime, ContractName);
        }
        else
        {
            await creditService.Create(client, program, Amount, sysTime);
        }
        return RedirectToPage("ClientPrograms", new { id = ClientId });
    }
}
