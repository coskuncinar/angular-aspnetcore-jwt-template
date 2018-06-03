using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.API.Auth
{
    public class AuthResponse
    {
        public int id { get; set; }
        public Token access_token { get; set; }
        public Token refresh_token { get; set; }
    }
}
