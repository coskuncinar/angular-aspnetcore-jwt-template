using Microsoft.IdentityModel.Tokens;

namespace Website.API.Auth
{
    public class JwtOptions
    {
        public string Issuer { get; set; }
        public string Subject { get; set; }
        public string Audience { get; set; }
        public SigningCredentials SigningCredentials { get; set; }

        public int AccessTokeTimeToLiveInMinutes { get; set; } = 5;
        public int RefreshTokeTimeToLiveInDays { get; set; } = 7;
    }
}
