using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniAccountManagementSystem.Data;
using MiniAccountManagementSystem.Entity;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MiniAccountManagementSystem.Pages.Accountant
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
        public List<Voucher>? voucherList = new();

        public List<SelectListItem> AccountList { get; set; }

        public void OnGet()
        {
            LoadAccounts();
            Voucher = new Voucher
            {
                Entries = new List<VoucherEntries>
            {
                new VoucherEntries()
            }
            };
        }

        public void OnPost()
        {
            LoadAccounts();

            try
            {
                _db.SaveVoucher(Voucher);
                TempData["Message"] = "Voucher entry success.";
            }
            catch (Exception ex)
            {
                // Log ex.Message or add ModelState error
                ModelState.AddModelError("", "Failed to save voucher: " + ex.Message);

            }

        }


        private void LoadAccounts()
        {

            AccountList = _db.GetIdNameList("sp_GetParentAccount")
                     .Select(r => new SelectListItem { Value = r.Id, Text = r.Name }).ToList();

        }
    }
}
