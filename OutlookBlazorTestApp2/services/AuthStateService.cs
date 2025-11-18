using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Web;
using System.Security.Claims;

namespace OutlookBlazorTestApp2.Services
{
    public class AuthStateService
    {
        private readonly IHttpContextAccessor _http;
        public AuthStateService(IHttpContextAccessor http)
        {
            _http = http;
        }

        public bool IsLoggedIn => _http.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public string CurrentUserDisplayName =>
            _http.HttpContext?.User?.FindFirst("name")?.Value
            ?? _http.HttpContext?.User?.Identity?.Name
            ?? "Unknown";

        public string CurrentUserEmail =>
            _http.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value
            ?? _http.HttpContext?.User?.FindFirst("preferred_username")?.Value
            ?? "";

        public IEnumerable<string> GetRoles()
        {
            var u = _http.HttpContext?.User;
            if (u == null) return Enumerable.Empty<string>();
            // roles claim might be "roles" or "groups"
            var roles = u.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            if (!roles.Any())
            {
                roles = u.FindAll("roles").Select(c => c.Value).ToList();
            }
            return roles;
        }

        // Programmatic sign out (server side)
        public async Task SignOutAsync()
        {
            if (_http.HttpContext != null)
            {
                await _http.HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
                await _http.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }
    }
}



