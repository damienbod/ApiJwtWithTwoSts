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
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            var authProperties = new AuthenticationProperties
            {
                RedirectUri = "/"
            };

            // custom claims added to idp, you need to implement something on your idp for this
            var usedT1ForAuthn = User.Claims.Any(idpClaim => idpClaim.Type == "idp" && idpClaim.Value == "T1");
            var usedT2ForAuthn = User.Claims.Any(idpClaim => idpClaim.Type == "idp" && idpClaim.Value == "T2");

            if (usedT1ForAuthn)
                return SignOut(authProperties, CookieAuthenticationDefaults.AuthenticationScheme, "T1");

            if (usedT2ForAuthn)
                return SignOut(authProperties, CookieAuthenticationDefaults.AuthenticationScheme, "T2");

            return SignOut(authProperties, CookieAuthenticationDefaults.AuthenticationScheme);
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