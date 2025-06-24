using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniAccountManagementSystem.Data;
using MiniAccountManagementSystem.Entity;
using System.Data;
using System.Reflection;


namespace MiniAccountManagementSystem.Pages.ChartOfAccounts
{
    public class ChartOfAccountsModel : PageModel
    {
        private readonly DbHelper _db;

        public ChartOfAccountsModel(IConfiguration config)
        {
            _db = new DbHelper(config);
        }

        public List<Account> Accounts { get; set; }

        [BindProperty] public int? EditingId { get; set; }
        [BindProperty] public string AccountName { get; set; }
        [BindProperty] public int? ParentAccountID { get; set; }
        public List<SelectListItem> ParentAccounts { get; set; }
        [BindProperty] public string AccountType { get; set; }

        public void OnGet()
        {
            Accounts = _db.GetChartOfAccounts();
            LoadDropdowns();
        }

        private void LoadDropdowns()
        {
            ParentAccounts = _db.GetIdNameList("sp_GetParentAccount")
                .Select(r => new SelectListItem { Value = r.Id, Text = r.Name }).ToList();

            ParentAccounts.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "-- Select --"
            });

        }

        public IActionResult OnPostAdd()
        {
            _db.ManageChartOfAccount("Insert", null, AccountName, ParentAccountID, AccountType);
            TempData["Message"] = "Account created successfully.";
            return RedirectToPage();
        }

        public IActionResult OnPostEdit()
        {
            _db.ManageChartOfAccount("Update", EditingId, AccountName, ParentAccountID, AccountType);
            TempData["Message"] = "Account updated successfully.";
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            _db.ManageChartOfAccount("Delete", id, null, null, null);
            TempData["Message"] = "Account deleted successfully.";
            return RedirectToPage();
        }
    }


}
