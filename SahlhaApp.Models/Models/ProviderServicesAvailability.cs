using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.Models
{
    public class ProviderServicesAvailability
    {
        public int Id { get; set; }
        public string Day { get; set; }
        //Navigation Prop 
        public int ProviderId { get; set; }
        public Provider Provider { get; set; }
    }
}
