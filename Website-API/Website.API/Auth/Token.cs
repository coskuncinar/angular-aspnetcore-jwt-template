using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.API.Auth
{
    public class Token
    {
        public string TokenHash { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
