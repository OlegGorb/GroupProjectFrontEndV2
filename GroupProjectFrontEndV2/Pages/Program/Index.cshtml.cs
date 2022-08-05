using GroupProjectFrontEndV2.Helpers;
using GroupProjectFrontEndV2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Http.Headers;

namespace GroupProjectFrontEndV2.Pages.Program
{
    [Authorize(Policy = "Admin")]
    public class IndexModel : PageModel
    {
        [BindProperty]
        public List<StudentProgram> Programs { get; set; }

        [BindProperty]
        public StudentProgram NewProgram { get; set; }

        HttpClient httpClient;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient("BackendApi");
        }

        public async Task OnGetAsync()

        {
            string token = HttpContext.Request.Cookies[Constants.XAccessToken];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Programs = await httpClient.GetFromJsonAsync<List<StudentProgram>>("/api/Program/GetPrograms");
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (NewProgram.Name != null && NewProgram.Name != "")
            {
                var result = await httpClient.PostAsJsonAsync<StudentProgram>("/api/Program/AddProgram", NewProgram);

                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return RedirectToPage("/Program/Index");
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
