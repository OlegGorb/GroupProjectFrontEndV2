using GroupProjectFrontEndV2.Helpers;
using GroupProjectFrontEndV2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace GroupProjectFrontEndV2.Pages.Program
{
    [Authorize(Policy = "Admin")]
    public class EditModel : PageModel
    {
        [BindProperty]
        public StudentProgram Program { get; set; }

        private HttpClient httpClient;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient("BackendApi");
        }

        public async Task OnGetAsync(int id)
        {
            string token = HttpContext.Request.Cookies[Constants.XAccessToken];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (id != null)
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(httpClient.BaseAddress.ToString() + "api/Program/GetProgram"),
                    Content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json")
                };

                var response = await httpClient.SendAsync(request);

                var result = await response.Content.ReadAsStringAsync();


                Program = JsonConvert.DeserializeObject<StudentProgram>(result);
                Console.WriteLine("Hello");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (ModelState.IsValid)
            {
                var result = await httpClient.PutAsJsonAsync<StudentProgram>("/api/Program/EditProgram", Program);

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
