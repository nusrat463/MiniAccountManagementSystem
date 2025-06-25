namespace MiniAccountManagementSystem.Entity
{
    public class RoleModuleAccess
    {
        public int ModuleId { get; set; }
        public bool CanView { get; set; }
        public bool CanEdit { get; set; }
    }
}
