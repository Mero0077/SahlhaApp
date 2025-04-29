using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.Models
{
    public class SubService
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public TimeOnly EstimatedTime { get; set; }
        public decimal? BasePrice { get; set; }
        public bool Status { get; set; } = true;
        //Navigation Prop 
        public ICollection<Provider>? Providers { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
