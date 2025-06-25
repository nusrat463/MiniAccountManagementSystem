using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MiniAccountManagementSystem.Pages.Admin
{
    public class UsersModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public List<UserViewModel> Users { get; set; }
        public List<string> AllRoles { get; set; }

        public async Task OnGetAsync()
        {
            AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            Users = new List<UserViewModel>();

            foreach (var user in _userManager.Users.ToList())
            {
                var roles = await _userManager.GetRolesAsync(user);
                Users.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    CurrentRole = roles.FirstOrDefault() ?? "None"
                });
            }
        }

        public async Task<IActionResult> OnPostChangeRoleAsync(string UserId, string NewRole)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, NewRole);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteUserAsync(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToPage();
        }

        public class UserViewModel
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public string CurrentRole { get; set; }
        }
    }
}
