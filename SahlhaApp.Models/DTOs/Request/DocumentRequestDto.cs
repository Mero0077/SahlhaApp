using Microsoft.AspNetCore.Http;
using SahlhaApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.DTOs.Request
{
    public class DocumentRequestDto
    {
        public List<IFormFile> File { get; set; }
    }
}
