using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.DTOs.Request.PasswordRequests
{
    public class UpdatePasswordRequestDto
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }
    }
}
