using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.Models
{
    public class MonthlySubscription
    {
        public int Id { get; set; }
        public decimal? Price { get; set; }
        //Navigation Prop
        
        public Provider Provider { get; set; }
        public int ProviderId { get; set; }
    }
}
