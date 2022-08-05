using GroupProjectFrontEndV2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

namespace GroupProjectFrontEndV2.Pages.Account
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public User User { get; set; }

        private HttpClient httpClient;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient("BackendApi");
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (ModelState.IsValid)
            {
                var result = await httpClient.PostAsJsonAsync<User>("/api/Users/Register", User);

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return RedirectToPage("/Index");
                }
                else
                {
                    return RedirectToPage("/Error");
                }
            }

            return RedirectToPage("/Error");
        }
    }
}
