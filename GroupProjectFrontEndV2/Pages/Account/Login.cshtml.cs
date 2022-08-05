using GroupProjectFrontEndV2.Auth;
using GroupProjectFrontEndV2.Helpers;
using GroupProjectFrontEndV2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;

namespace GroupProjectFrontEndV2.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public User User { get; set; }
        private HttpClient httpClient;
        public LoginModel(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient("BackendApi");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("/Error");
            }

            // Send Login request to Backend
            var result = await httpClient.PostAsJsonAsync<User>("/api/Users/Login", User);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                return RedirectToPage("/Error");
            }

            // Save token to cookie
            var strJwt = await result.Content.ReadAsStringAsync();
            JwtToken token = JsonConvert.DeserializeObject<JwtToken>(strJwt);
            HttpContext.Response.Cookies.Append(
                Constants.XAccessToken,
                token.AccessToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                });

            // Save session id as cookie
            HttpContext.Response.Cookies.Append(
                Constants.Session,
                token.Session.Id.ToString(),
                new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                });

            HttpContext.Response.Cookies.Append(
                Constants.UserId,
                token.User.Id.ToString(),
                new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(30)
                });

            // Authenticate user with cookie
            List<Claim> claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, token.User.FirstName + " " + token.User.LastName),
                        new Claim("Admin", token.User.Admin.ToString())
                    };
            ClaimsIdentity identity = new ClaimsIdentity(claims, "MyCookieAuth");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);


            return RedirectToPage("/Index");
        }
    }
}
