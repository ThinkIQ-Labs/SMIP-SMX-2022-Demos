using Newtonsoft.Json;

namespace smip.sdk
{
    public class Authenticator
    {

        [JsonProperty("graphQlEndpoint")]
        public string GraphQlEndpoint { get; set; }

        [JsonProperty("clientId")]
        public string ClientId { get; set; }

        [JsonProperty("clientSecret")]
        public string ClientSecret { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        public Authenticator()
        {

        }
    }
}
