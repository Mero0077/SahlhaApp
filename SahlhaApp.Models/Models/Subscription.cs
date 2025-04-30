using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int ProviderId { get; set; }
        public Provider Provider { get; set; }
    }
}
