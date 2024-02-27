using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebBank.AppCore.Entities;
using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;

namespace WebBank.Pages;

public class DepositContractModel(MySQLContext context, IDepositService depositService, ITimeService timeService) : PageModel
{
    private readonly IDepositService _depositService = depositService;
    private readonly MySQLContext _context = context;

    public DateOnly SystemDate { get; } = timeService.GetSystemDate();

    [BindProperty]
    public int ClientId { get; set; }
    public string ClientName { get; set; } = "";

    [BindProperty]
    public string ContractName { get; set; } = string.Empty;

    [BindProperty]
    public decimal StartPayment { get; set; } = 0;

    [BindProperty]
    public int? ChoosenProgram { get; set; }
    public List<DepositProgram> DepositPrograms { get; set; } = [];

    public async Task <IActionResult> OnGetAsync(int clientId)
    {
        ClientId = clientId;
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == clientId);
        if (client == null || !client.IsActive)
        {
            return RedirectToPage("Error");
        }
        ClientName = client.Surname + " " + client.Name + " " + client.Patronymic;

        DepositPrograms = [.. _context.DepositPrograms];
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ChoosenProgram == null)
        {
            return RedirectToPage("Error");
        }

        var client = _context.Clients.FirstOrDefault(c => c.Id == ClientId);
        var program = _context.DepositPrograms.FirstOrDefault(c => c.Id == ChoosenProgram.Value);
        if (program == null || client == null)
        {
            return RedirectToPage("Error");
        }

        var sysTime = new DateTime(SystemDate.Year, SystemDate.Month, SystemDate.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        if (ContractName != null && ContractName != string.Empty)
        {
            await _depositService.Create(client, program, StartPayment, sysTime, ContractName);
        }
        else
        {
            await _depositService.Create(client, program, StartPayment, sysTime);
        }
        return RedirectToPage("ClientPrograms", new { id = ClientId });
    }
}
