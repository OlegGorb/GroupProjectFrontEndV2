using GroupProjectFrontEndV2.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace GroupProjectFrontEndV2.Pages.Account
{
    public class LogoutModel : PageModel
    {
        HttpClient httpClient;
        public LogoutModel(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient("BackendApi");
        }

        public async Task<IActionResult> OnGet()
        {
            string token = HttpContext.Request.Cookies[Constants.XAccessToken];
            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            string session = HttpContext.Request.Cookies[Constants.Session];
            if (session != null)
            {
                var result = await httpClient.PostAsJsonAsync<int>("/api/Users/Logout", Int32.Parse(session));
                Console.WriteLine(result.IsSuccessStatusCode);
            }

            HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToPage("/Index");
        }
    }
}
