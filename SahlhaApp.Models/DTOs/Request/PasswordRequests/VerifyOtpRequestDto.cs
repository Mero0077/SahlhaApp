using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.DTOs.Request.PasswordRequests
{
    public class VerifyOtpRequestDto
    {
        public string Email { get; set; }
        public int Otp { get; set; }
    }
}
