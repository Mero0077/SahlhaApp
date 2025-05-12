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
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; } = true;
        public decimal Duration { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public decimal BasePrice { get; set; }
        public TimeOnly EstimatedTime { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
        public ICollection<ProviderSubServices> ProviderServices { get; set; }
        public ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}