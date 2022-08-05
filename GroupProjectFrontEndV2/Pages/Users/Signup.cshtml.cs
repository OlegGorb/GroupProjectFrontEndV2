using GroupProjectFrontEndV2.Helpers;
using GroupProjectFrontEndV2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

namespace GroupProjectFrontEndV2.Pages.Users
{
    public class SignupModel : PageModel
    {
        [BindProperty]
        public List<StudentProgram> Programs { get; set; }


        [BindProperty]
        public User User { get; set; }



        HttpClient httpClient;

        public SignupModel(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient("BackendApi");
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            string token = HttpContext.Request.Cookies[Constants.XAccessToken];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Get Programs
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(httpClient.BaseAddress.ToString() + "api/Program/GetPrograms")
            };

            var response = await httpClient.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            Programs = JsonConvert.DeserializeObject<List<StudentProgram>>(result);

            // Get User Info
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            User = await httpClient.GetFromJsonAsync<User>($"/api/Users/{HttpContext.Request.Cookies[Constants.UserId]}");

            if (id != null && id != 0)
            {
                UserCourse userCourse = new UserCourse()
                {
                    UserId = User.Id
                };

                foreach (StudentProgram program in Programs)
                {
                    if (program.Id == id)
                    {
                        userCourse.CourseId = program.Id;
                        break;
                    }
                }

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var postResult = await httpClient.PostAsJsonAsync<UserCourse>("api/Program/SignUp", userCourse);


                if (postResult.StatusCode == HttpStatusCode.OK)
                {
                    return RedirectToPage("/Users/MyPage");
                }
                else
                {
                    return RedirectToPage("/Error");
                }


                return RedirectToPage("/Error");
            }

            return Page();

        }

        public async void OnPostAsync(int programId)
        {

        }
    }
}
