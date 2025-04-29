using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        //Navigation Prop 
        public Payment Payment { get; set; }

    }
}
