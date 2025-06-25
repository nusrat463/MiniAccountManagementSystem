using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniAccountManagementSystem.Data;
using MiniAccountManagementSystem.Entity;
using System.Collections.Generic;

public class AssignAccessModel : PageModel
{
    private readonly DbHelper _dbHelper;

    public AssignAccessModel(DbHelper dbHelper)
    {
        _dbHelper = dbHelper;
    }

    [BindProperty]
    public string SelectedRoleId { get; set; }

    [BindProperty]
    public string SelectedModuleName { get; set; }


    [BindProperty]
    public bool CanView { get; set; }

    [BindProperty]
    public bool CanEdit { get; set; }

    public List<(string Id, string Name)> Roles { get; set; } = new();
    public List<(string Id, string Name)> ModuleList { get; set; } = new();


    public void OnGet()
    {
        LoadDropdowns();
    }

    public IActionResult OnPost()
    {
        LoadDropdowns();

        if (!string.IsNullOrEmpty(SelectedRoleId) && !string.IsNullOrEmpty(SelectedModuleName))
        {
            _dbHelper.AssignModuleAccess(SelectedRoleId, SelectedModuleName, CanView, CanEdit);
            ViewData["Message"] = "Access assigned successfully.";
        }

        return RedirectToPage();
    }


    private void LoadDropdowns()
    {
        Roles = _dbHelper.GetIdNameList("sp_getRoles");
        ModuleList = _dbHelper.GetIdNameList("sp_getModules");
    }
}

