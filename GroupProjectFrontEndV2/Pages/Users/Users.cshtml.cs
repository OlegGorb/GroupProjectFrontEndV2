using GroupProjectFrontEndV2.Helpers;
using GroupProjectFrontEndV2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace GroupProjectFrontEndV2.Pages.Users
{
    [Authorize(Policy = "Admin")]
    public class UsersModel : PageModel
    {
        [BindProperty]
        public List<User> Users { get; set; }

        HttpClient httpClient;

        public UsersModel(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient("BackendApi");
        }

        public async Task OnGetAsync()
        {
            string token = HttpContext.Request.Cookies[Constants.XAccessToken];

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            Users = await httpClient.GetFromJsonAsync<List<User>>("/api/Users/GetUsers");
        }
    }
}
