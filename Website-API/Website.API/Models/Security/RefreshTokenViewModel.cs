using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Website.API.Models.Security
{
    public class RefreshTokenViewModel
    {
        [Required]
        public string TokenHash { get; set; }
    }
}
