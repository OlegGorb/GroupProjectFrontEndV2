using GroupProjectFrontEndV2.Helpers;
using GroupProjectFrontEndV2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace GroupProjectFrontEndV2.Pages.Users
{
    public class MyPageModel : PageModel
    {
        [BindProperty]
        public User User { get; set; }

        [BindProperty]
        public int Id { get; set; }

        HttpClient httpClient;

        public MyPageModel(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient("BackendApi");
        }

        public async Task OnGetAsync(int id)
        {
            if (id != null && id != 0)
            {
                Id = id;
            }


            string token = HttpContext.Request.Cookies[Constants.XAccessToken];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (id != null && id != 0)
            {
                User = await httpClient.GetFromJsonAsync<User>($"/api/Users/{id}");
            }
            else
            {
                User = await httpClient.GetFromJsonAsync<User>($"/api/Users/{HttpContext.Request.Cookies[Constants.UserId]}");
            }

        }
    }
}
