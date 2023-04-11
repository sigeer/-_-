using Newtonsoft.Json;

namespace DDDApplication.Contract.Auth
{
    public class TokenModel
    {
        [JsonProperty("access_token")]
        public string? AccessToken { get; set; }
        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }
    }
    public class AuthSettings
    {
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        /// <summary>
        /// unit. hour
        /// </summary>
        public int Expire { get; set; }
        public int RefreshExpire { get; set; }
        public string IssuerSigningKey { get; set; } = null!;
    }
}
