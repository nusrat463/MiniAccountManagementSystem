using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniAccountManagementSystem.Entity;

namespace MiniAccountManagementSystem.Pages.VoucherEntry
{
    public class VoucherEntryModel : PageModel
    {
        [BindProperty]
        public Voucher Voucher { get; set; }
        public void OnGet()
        {
        }
    }
}
