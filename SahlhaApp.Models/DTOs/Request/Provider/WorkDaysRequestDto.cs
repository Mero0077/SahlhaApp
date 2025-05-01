using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.DTOs.Request.Provider
{
    public class WorkDaysRequestDto
    {
        public int ProviderId { get; set; }
        public List<string> WorkDays { get; set; } = new List<string>();
    }
}
