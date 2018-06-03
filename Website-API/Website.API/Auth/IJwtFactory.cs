using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Website.API.Auth
{
    public interface IJwtFactory
    {
        Token GenerateAccessToken(string userName, ClaimsIdentity identity);
        Token GenerateRefreshToken(string userName, int id);
        ClaimsIdentity GenerateClaimsIdentity(string userName, string id);

        RefreshToken GetRefreshToken(string token);
        void RevokeRefreshToken(RefreshToken token);
    }
}
