using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.DTOs.Request.Profile
{
    public class ProfileRequestDto
    {
        public string UserName { get; set; }
        public string? Email { get; set; }
    }
}
