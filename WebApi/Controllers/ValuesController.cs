using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
public class ValuesController : Controller
{
    [Authorize(AuthenticationSchemes = "SchemeStsA,SchemeStsB", Policy = "MyPolicy")]
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return ["data 1 from the api", "data 2 from the api"];
    }
}
