using Microsoft.AspNetCore.Http;
using SahlhaApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.DTOs.Request.Provider
{
    public class ProviderRequestDto
    {
        public DateTime appliedAt { get; set; } = DateTime.Now;
        public IFormFile Id { get; set; }
        public IFormFile BirthCertificate { get; set; }
        public IFormFile CriminalRecord { get; set; }
        public string? ApplicationUserId { get; set; }
    }
}
