using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.DTOs.Request.Provider
{
    public class ProviderRequestDto
    {
        public string Description { get; set; }
        public decimal HourlyRate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public string ProviderId { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
