using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.DTOs.Request
{
    public class UpdateProfilePictureRequestDto
    {
        public string Email { get; set; }
        public IFormFile ImgUrl { get; set; }
    }
}
