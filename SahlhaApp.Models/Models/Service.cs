using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.Models
{
    public class Service
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool Status { get; set; }  = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        //Navigation Prop 
        public ICollection<SubService> SubServices { get; set; }

    }
}
