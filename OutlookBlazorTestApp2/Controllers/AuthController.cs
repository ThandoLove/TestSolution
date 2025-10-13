
using Microsoft.AspNetCore.Mvc;
using OutlookBlazorTestApp2.services;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OutlookBlazorTestApp2.Data.Models;
using OutlookBlazorTestApp2.Services;
using OutlookBlazorTestApp2.Data.Models.Auth;


namespace OutlookBlazorTestApp2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly IAuthService _auth;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService auth, ILogger<AuthController> logger)
        {
            _auth = auth;
            _logger = logger;
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn()
        {
            var result = await _auth.SignInAsync();
            return Ok(new ApiResponse<AuthResult> { Data = result });
        }

        [HttpPost("signout")]
        public async Task<IActionResult> SignOut()
        {
            await _auth.SignOutAsync();
            return Ok(new ApiResponse<bool> { Data = true });
        }

        [HttpGet("is-authenticated")]
        public async Task<IActionResult> IsAuthenticated()
        {
            var isAuthenticated = await _auth.IsAuthenticatedAsync();
            return Ok(new ApiResponse<bool> { Data = isAuthenticated });
        }

        [HttpGet("user-profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var result = await _auth.GetUserProfileAsync();
            return Ok(new ApiResponse<Data.Models.UserProfile> { Data = result });
        }

        [HttpGet("auth-state")]
        public async Task<IActionResult> GetAuthState()
        {
            var result = await _auth.GetAuthStateAsync();
            return Ok(new ApiResponse<AuthState> { Data = result });
        }
    }
}