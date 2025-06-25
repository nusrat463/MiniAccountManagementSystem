using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MiniAccountManagementSystem.Pages.Admin
{
    public class RolesModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public List<IdentityRole> Roles { get; set; }

        public void OnGet()
        {
            Roles = _roleManager.Roles.ToList();
        }

        public async Task<IActionResult> OnPostCreateRoleAsync(string NewRoleName)
        {
            if (!string.IsNullOrWhiteSpace(NewRoleName))
            {
                var exists = await _roleManager.RoleExistsAsync(NewRoleName);
                if (!exists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(NewRoleName));
                }
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteRoleAsync(string RoleId)
        {
            var role = await _roleManager.FindByIdAsync(RoleId);
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
            }

            return RedirectToPage();
        }
    }
}
