using GroupProjectFrontEndV2.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace GroupProjectFrontEndV2.Pages.Program
{
    [Authorize(Policy = "Admin")]
    public class DeleteModel : PageModel
    {
        HttpClient httpClient;

        public DeleteModel(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient("BackendApi");
        }
        public async Task<IActionResult> OnGet(int id)
        {
            string token = HttpContext.Request.Cookies[Constants.XAccessToken];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(httpClient.BaseAddress.ToString() + "api/Program/DeleteProgram"),
                Content = new StringContent(JsonConvert.SerializeObject(id), Encoding.UTF8, "application/json")
            };
            var response = await httpClient.SendAsync(request);

            return RedirectToPage("/Program/Index");
        }
    }
}
