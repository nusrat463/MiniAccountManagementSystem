using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MiniAccountManagementSystem.Data;

namespace MiniAccountManagementSystem.Pages.Admin
{
    public class AssignAccessModel : PageModel
    {
        private readonly DbHelper _dbHelper;

        public AssignAccessModel(IConfiguration configuration)
        {
            _dbHelper = new DbHelper(configuration);
        }

        [BindProperty]
        public string SelectedRoleId { get; set; }

        [BindProperty]
        public string SelectedModuleId { get; set; }

        public List<SelectListItem> Roles { get; set; }
        public List<SelectListItem> Modules { get; set; }

        public void OnGet()
        {
            LoadDropdowns();
        }

        private void LoadDropdowns()
        {
            Roles = _dbHelper.GetIdNameList("sp_GetRoles")
                .Select(r => new SelectListItem { Value = r.Id, Text = r.Name }).ToList();

            Modules = _dbHelper.GetIdNameList("sp_GetModules")
                .Select(m => new SelectListItem { Value = m.Id, Text = m.Name }).ToList();
        }

        public IActionResult OnPost()
        {
            if (!string.IsNullOrEmpty(SelectedRoleId) && int.TryParse(SelectedModuleId, out int moduleId))
            {
                _dbHelper.AssignModuleAccess(SelectedRoleId, moduleId);
                TempData["Message"] = "Access assigned successfully.";
            }

            LoadDropdowns();
            return Page();
        }
    }
}
