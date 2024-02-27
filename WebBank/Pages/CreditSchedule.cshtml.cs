using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebBank.AppCore.Entities;
using WebBank.Infrastructure.Data;

namespace WebBank.Pages
{
    public class CreditScheduleModel(MySQLContext context) : PageModel
    {
        private readonly MySQLContext _context = context;

        public List<CreditSchedule> Schedule = [];
        public async Task<IActionResult> OnGetAsync(int creditId)
        {
            var credit = await _context.ClientCredits.FirstOrDefaultAsync(cc => cc.Id == creditId && cc.IsActive);
            if (credit == null)
            {
                return NotFound();
            }

            Schedule = await _context.Schedules
                .Where(s => s.Credit.Id == creditId)
                .OrderBy(s => s.Date)
                .ToListAsync();
            return Page();
        }
    }
}
