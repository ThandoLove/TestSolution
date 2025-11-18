using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;

namespace OutlookBlazorTestApp2.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult SignIn(string? returnUrl = "/")
        {
            var props = new AuthenticationProperties { RedirectUri = returnUrl ?? "/" };
            return Challenge(props, OpenIdConnectDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public IActionResult SignOutLocal()
        {
            var callbackUrl = Url.Page("/Index", pageHandler: null, values: null, protocol: Request.Scheme);
            return SignOut(new AuthenticationProperties { RedirectUri = callbackUrl }, OpenIdConnectDefaults.AuthenticationScheme, "Cookies");
        }
    }
}

