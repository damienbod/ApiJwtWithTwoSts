using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace BlazorClient.Server.Controllers
{
    [ValidateAntiForgeryToken]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class DirectApiController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get() => new List<string> { "some data", "more data", "loads of data" };
    }
}