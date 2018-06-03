using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Website.API.Auth
{
    public class JwtFactory : IJwtFactory
    {
        private readonly JwtOptions jwtOptions;

        private ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        private ICollection<Token> BlackList { get; set; } = new List<Token>();

        public JwtFactory(IOptions<JwtOptions> jwtOptions)
        {
            this.jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(this.jwtOptions);
        }

        public Token GenerateAccessToken(string userName, ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;

            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub, userName),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(now).ToString(), ClaimValueTypes.Integer64),
                 identity.FindFirst(Helpers.Constants.Strings.JwtClaimIdentifiers.Rol),
                 identity.FindFirst(Helpers.Constants.Strings.JwtClaimIdentifiers.Id)
            };

            var expiresAt = now.AddMinutes(jwtOptions.AccessTokeTimeToLiveInMinutes);

            var jwt = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claims,
                notBefore: now,
                expires: expiresAt,
                signingCredentials: jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var token = new Token()
            {
                TokenHash = encodedJwt,
                ExpiresAt = expiresAt
            };

            return token;
        }

        public ClaimsIdentity GenerateClaimsIdentity(string userName, string id)
        {
            return new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
            {
                new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.Id, id),
                new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.Rol, Helpers.Constants.Strings.JwtClaims.ApiAccess)
            });
        }

        public Token GenerateRefreshToken(string userName, int id)
        {
            var now = DateTime.UtcNow;

            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub, userName),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(now).ToString(), ClaimValueTypes.Integer64)
            };

            var expiresAt = now.AddDays(jwtOptions.RefreshTokeTimeToLiveInDays);

            var jwt = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claims,
                notBefore: now,
                expires: expiresAt,
                signingCredentials: jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var token = new RefreshToken()
            {
                Id = id,
                TokenHash = encodedJwt,
                ExpiresAt = expiresAt
            };

            this.RefreshTokens.Add(token);

            var result = new Token()
            {
                TokenHash = token.TokenHash,
                ExpiresAt = token.ExpiresAt
            };

            return result;
        }

        public RefreshToken GetRefreshToken(string token)
        {
            var refreshToken = this.RefreshTokens.SingleOrDefault(x => x.TokenHash == token);

            if (refreshToken == null)
            {
                return null;
            }

            var result = this.BlackList.Any(x => x == refreshToken);

            if(result)
            {
                return null;
            }

            return refreshToken;
        }

        public void RevokeRefreshToken(RefreshToken token)
        {
            this.RefreshTokens.Remove(token);
        }

        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                               .TotalSeconds);

        private static void ThrowIfInvalidOptions(JwtOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtOptions.SigningCredentials));
            }
        }
    }
}
