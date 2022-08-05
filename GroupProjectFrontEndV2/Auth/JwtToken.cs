using GroupProjectFrontEndV2.Models;
using Newtonsoft.Json;

namespace GroupProjectFrontEndV2.Auth
{
    public class JwtToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("current_session")]
        public TimeSpend Session { get; set; }
    }
}
