namespace MiniAccountManagementSystem.Entity
{
    public class RoleModuleAccess
    {
        public string ModuleName { get; set; }
        public bool CanView { get; set; }
        public bool CanEdit { get; set; }
    }
}
