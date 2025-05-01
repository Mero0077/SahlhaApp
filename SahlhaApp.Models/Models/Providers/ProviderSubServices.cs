using SahlhaApp.Models.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.Models.Providers
{
    public class ProviderSubServices
    {
        public int Id { get; set; }
        public int SubServiceId { get; set; }
        public SubService SubService { get; set; }
        public int ProviderId { get; set; }
        public Provider Provider { get; set; }
    }
}
