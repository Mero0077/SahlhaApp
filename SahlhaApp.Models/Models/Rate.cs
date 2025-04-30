using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.Models
{
    public class Rate
    {
        public int Id { get; set; }
        public string? Comment { get; set; }
        public double? RateValue { get; set; }
        //Navigation Prop
        public string ApplicationUserId { get; set; }
        public int ProviderId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Provider Provider { get; set; }

    }
}
