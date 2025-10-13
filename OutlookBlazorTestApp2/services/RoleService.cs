namespace OutlookBlazorTestApp2.services
{
    public class RoleService
    {
        public string CurrentUserRole { get; set; } = "Employee"; // Default role

        public bool IsManager()
        {
            return CurrentUserRole.Equals("Manager", StringComparison.OrdinalIgnoreCase);
        }

        public bool IsAdmin()
        {
            return CurrentUserRole.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        }

        public bool IsEmployee()
        {
            return CurrentUserRole.Equals("Employee", StringComparison.OrdinalIgnoreCase);
        }
    }
}

