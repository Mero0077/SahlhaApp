using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SahlhaApp.Models.Models
{
    [PrimaryKey(nameof(SubServiceId), nameof(ProviderId))]

    public class ProviderSubService
    {
        public Provider Provider { get; set; }
        public int ProviderId { get; set; }
        public SubService SubService { get; set; }
        public int SubServiceId { get; set; }
    }
}
