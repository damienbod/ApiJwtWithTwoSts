using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPageOidcClient.Pages;

[Authorize]
public class IndexModel : PageModel
{
    private readonly ApiService _apiService;

    public List<string> Data { get; set; } = new List<string>();
    public IndexModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task OnGetAsync()
    {
        //var result = await _apiService.GetUnsecureApiDataAsync();
        var resultSecure = await _apiService.GetApiDataAsync();
        if (resultSecure != null)
            Data = resultSecure;

        Console.WriteLine(resultSecure!.FirstOrDefault());
    }
}
