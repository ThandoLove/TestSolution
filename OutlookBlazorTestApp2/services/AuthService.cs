using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using OutlookBlazorTestApp2.Data.Models;
using OutlookBlazorTestApp2.Data.Models.Auth;


namespace OutlookBlazorTestApp2.Services
{
    public interface IAuthService
    {
        Task<AuthResult> SignInAsync();
        Task SignOutAsync();
        Task<bool> IsAuthenticatedAsync();
        Task<UserProfile> GetUserProfileAsync();
        Task<string> GetAccessTokenAsync(string[] scopes);
        Task<AuthState> GetAuthStateAsync();
    }

    public class AuthService : IAuthService
    {
        private readonly AuthenticationStateProvider _provider;
        private readonly IHttpContextAccessor _http;
        private readonly ILogger<AuthService> _logger;
        private readonly IJSRuntime _jsRuntime;

        public AuthService(
            AuthenticationStateProvider provider,
            IHttpContextAccessor http,
            ILogger<AuthService> logger,
            IJSRuntime jsRuntime)
        {
            _provider = provider;
            _http = http;
            _logger = logger;
            _jsRuntime = jsRuntime;
        }

        public async Task<AuthResult> SignInAsync()
        {
            try
            {
                // For now, mock sign-in process
                var authState = await GetAuthStateAsync();

                return new AuthResult
                {
                    Success = authState.IsAuthenticated,
                    State = authState,
                    ErrorMessage = authState.IsAuthenticated ? null : "Authentication failed"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during sign-in");
                return new AuthResult
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task SignOutAsync()
        {
            try
            {
                _logger.LogInformation("Sign-out requested");
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during sign-out");
            }
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var authState = await GetAuthStateAsync();
            return authState.IsAuthenticated;
        }

        public async Task<UserProfile> GetUserProfileAsync()
        {
            try
            {
                var authState = await _provider.GetAuthenticationStateAsync();
                var user = authState.User;

                if (user.Identity?.IsAuthenticated != true)
                    return new UserProfile();

                return new UserProfile
                {
                    UserId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? "",
                    UserName = user.FindFirstValue(ClaimTypes.Name) ?? "",
                    Email = user.FindFirstValue(ClaimTypes.Email) ?? "",
                    FirstName = user.FindFirstValue(ClaimTypes.GivenName) ?? "",
                    LastName = user.FindFirstValue(ClaimTypes.Surname) ?? "",
                    Roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList(),
                    LastLogin = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user profile");
                return new UserProfile();
            }
        }

        public Task<string> GetAccessTokenAsync(string[] scopes)
        {
            _logger.LogInformation("Access token requested for scopes: {Scopes}", string.Join(", ", scopes));
            return Task.FromResult("mock-access-token");
        }

        public async Task<AuthState> GetAuthStateAsync()
        {
            try
            {
                var authState = await _provider.GetAuthenticationStateAsync();
                var user = authState.User;

                var state = new AuthState
                {
                    IsAuthenticated = user.Identity?.IsAuthenticated ?? false,
                    UserId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? "",
                    UserName = user.FindFirstValue(ClaimTypes.Name) ?? "",
                    Email = user.FindFirstValue(ClaimTypes.Email) ?? "",
                    Roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList()
                };

                if (user.FindFirstValue("exp") is string expStr &&
                    long.TryParse(expStr, out var exp))
                {
                    state.TokenExpiry = DateTimeOffset.FromUnixTimeSeconds(exp).DateTime;
                }

                return state;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting auth state");
                return new AuthState();
            }
        }
    }

    

   
}

//For real world token exchange

/*using Microsoft.Identity.Web;
using Microsoft.Extensions.Logging;

public class AuthService : IAuthService
{
    private readonly ITokenAcquisition _tokenAcquisition;
    private readonly ILogger<AuthService> _logger;

    public AuthService(ITokenAcquisition tokenAcquisition, ILogger<AuthService> logger)
    {
        _tokenAcquisition = tokenAcquisition;
        _logger = logger;
    }
public async Task<string> GetAccessTokenAsync(string[] scopes)
{
    try
    {
        _logger.LogInformation("Access token requested for scopes: {Scopes}", string.Join(", ", scopes));

        // ✅ Actually request a token using Microsoft Identity
        var token = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);

        return token;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error getting access token");
        return string.Empty;
    }
}*/

