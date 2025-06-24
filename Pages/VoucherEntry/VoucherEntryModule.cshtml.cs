using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniAccountManagementSystem.Data;
using MiniAccountManagementSystem.Entity;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MiniAccountManagementSystem.Pages.VoucherEntry
{
    public class VoucherEntryModuleModel : PageModel
    {
        private readonly DbHelper _db;

        public VoucherEntryModuleModel(IConfiguration config)
        {
            _db = new DbHelper(config);
        }

        [BindProperty]
        public Voucher Voucher { get; set; }

        public List<SelectListItem> AccountList { get; set; }

        public void OnGet()
        {
            LoadAccounts();

            Voucher = new Voucher
            {
                Entries = new List<VoucherEntries>
            {
                new VoucherEntries(),
                new VoucherEntries()
            }
            };
        }

        public IActionResult OnPost(string action, int? removeIndex)
        {
            LoadAccounts();

            if (Voucher == null)
                Voucher = new Voucher();

            if (Voucher.Entries == null)
                Voucher.Entries = new List<VoucherEntries>();


            if (action == "add")
            {
                Voucher.Entries.Add(new VoucherEntries());
                return Page(); // Re-render with the new row preserved
            }

            if (removeIndex.HasValue && removeIndex >= 0 && removeIndex < Voucher.Entries.Count)
            {
                Voucher.Entries.RemoveAt(removeIndex.Value);
                return Page(); // Re-render with row removed
            }

            if (action == "save")
            {
                // Filter out empty entries
                var validEntries = Voucher.Entries
                    .Where(e => e.AccountId != 0 && (e.Debit > 0 || e.Credit > 0))
                    .ToList();

                if (!validEntries.Any())
                {
                    ModelState.AddModelError("", "Please enter at least one valid entry.");
                    return Page();
                }

                Voucher.Entries = validEntries;

                // TODO: Save with stored procedure

                return RedirectToPage("Success");
            }

            return Page();
        }

        private void LoadAccounts()
        {
            AccountList = new List<SelectListItem>();

            AccountList = _db.GetIdNameList("sp_GetParentAccount")
                     .Select(r => new SelectListItem { Value = r.Id, Text = r.Name }).ToList();
        }
    }
}
