using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.Models
{
    public class Order
    {
        public int Id { get; set; }
        public Provider Provider { get; set; }
        public ApplicationUser User { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }

    }
}
