using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniAccountManagementSystem.Data;
using MiniAccountManagementSystem.Entity;
using System.Data;
using System.Reflection;


namespace MiniAccountManagementSystem.Pages.Accountant
{
    public class ChartOfAccountsModel : PageModel
    {
        private readonly DbHelper _dbHelper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public bool CanView { get; private set; }
        public bool CanEdit { get; private set; }

        public ChartOfAccountsModel(DbHelper dbHelper, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbHelper = dbHelper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public List<Account> Accounts { get; set; }
        public Account SelectedAccount { get; set; }
        [BindProperty] public int? EditingId { get; set; }
        [BindProperty] public string AccountName { get; set; }
        [BindProperty] public int? ParentAccountID { get; set; }
        public List<SelectListItem> ParentAccounts { get; set; }
        [BindProperty] public string AccountType { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            
            var user = await _userManager.GetUserAsync(User);
            var roleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            if (roleName == null)
            {
                return Forbid(); 
            }

            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return NotFound("Role not found");
            }

            var roleId = role.Id;
            int moduleId = 3;

            var access = _dbHelper.GetModuleAccess(roleId, 3);

            if (access == null || !access.CanView)
            {
                return Forbid(); //redirect to an error page
            }

            CanView = access.CanView;
            CanEdit = access.CanEdit;

            LoadDropdowns();

            return Page();
        }

        private void LoadDropdowns()
        {
            Accounts = _dbHelper.GetChartOfAccounts();
            ParentAccounts = _dbHelper.GetIdNameList("sp_GetParentAccount")
                .Select(r => new SelectListItem { Value = r.Id, Text = r.Name }).ToList();

            ParentAccounts.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "-- Select --"
            });

        }

        public IActionResult OnPostAdd()
        {
            _dbHelper.ManageChartOfAccount("Insert", null, AccountName, ParentAccountID, AccountType);
            TempData["Message"] = "Account created successfully.";
            return RedirectToPage();
        }

        public IActionResult OnPostEdit()
        {
            _dbHelper.ManageChartOfAccount("Update", EditingId, AccountName, ParentAccountID, AccountType);
            TempData["Message"] = "Account updated successfully.";
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            _dbHelper.ManageChartOfAccount("Delete", id, null, null, null);
            TempData["Message"] = "Account deleted successfully.";
            return RedirectToPage();
        }
    }


}
