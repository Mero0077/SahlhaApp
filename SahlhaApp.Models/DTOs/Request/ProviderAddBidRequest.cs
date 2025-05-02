using SahlhaApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.DTOs.Request
{
    public class ProviderAddBidRequest
    {
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public int Duration { get; set; }
        public int JobId { get; set; }
        public int ProviderId { get; set; }
    }
}
