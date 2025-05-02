using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.DTOs.Request.Profile
{
    public class UpdateAddressRequestDto
    {
        public string ApplicationUserId { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? District { get; set; }
        public string? Street { get; set; }
        public string? ZipCode { get; set; }
        public int? BuildingNumber { get; set; }
    }
}
