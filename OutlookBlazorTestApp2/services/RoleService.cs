using OutlookBlazorTestApp2.Services;
using System.Security.Claims;

namespace OutlookBlazorTestApp2.services
{
    public class RoleService
    {
        private readonly AuthStateService _auth;

        public RoleService(AuthStateService auth)
        {
            _auth = auth;
        }

        public bool IsInRole(string role)
        {
            return _auth.GetRoles().Any(r => string.Equals(r, role, StringComparison.OrdinalIgnoreCase));
        }

        public bool IsManager() => IsInRole("Manager") || IsInRole("Admin");
        public bool IsAdmin() => IsInRole("Admin");
    }
}
