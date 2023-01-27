using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace BlazorClient.Server.Controllers
{
    // orig src https://github.com/berhir/BlazorWebAssemblyCookieAuth
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        [HttpGet("LoginT1")]
        public IActionResult T1(string returnUrl)
        {
            var redirectUrl = !string.IsNullOrEmpty(returnUrl) ? returnUrl : "/";
            return Challenge(new AuthenticationProperties { RedirectUri = redirectUrl }, "T1");
        }

        [HttpGet("LoginT2")]
        public IActionResult T2(string returnUrl)
        {
            var redirectUrl = !string.IsNullOrEmpty(returnUrl) ? returnUrl : "/";
            return Challenge(new AuthenticationProperties { RedirectUri = redirectUrl }, "T2");
        }

        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost("LogoutT1")]
        public IActionResult LogoutT1() => SignOut(new AuthenticationProperties
        {
            RedirectUri = "/"
        },
        CookieAuthenticationDefaults.AuthenticationScheme, "T1");

        [ValidateAntiForgeryToken]
        [Authorize]
        [HttpPost("LogoutT2")]
        public IActionResult LogoutT2() => SignOut(new AuthenticationProperties
        {
            RedirectUri = "/"
        },
        CookieAuthenticationDefaults.AuthenticationScheme, "T2");
    }
}