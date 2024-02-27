using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebBank.AppCore.Entities;
using WebBank.AppCore.Interfaces;
using WebBank.Infrastructure.Data;

namespace WebBank.Pages;

public class CreditScheduleModel(
    MySQLContext context,
    ITimeService timeService) : PageModel
{
    public DateOnly SystemDate => timeService.GetSystemDate();

    public List<CreditSchedule> Schedule = [];
    public async Task<IActionResult> OnGetAsync(int creditId)
    {
        var credit = await context.ClientCredits.FirstOrDefaultAsync(cc => cc.Id == creditId && cc.IsActive);
        if (credit == null)
        {
            return NotFound();
        }

        Schedule = await context.Schedules
            .Where(s => s.Credit.Id == creditId)
            .OrderBy(s => s.Date)
            .ToListAsync();
        return Page();
    }
}
