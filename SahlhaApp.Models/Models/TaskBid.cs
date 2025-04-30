using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.Models
{
    public class TaskBid
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int Duration { get; set; }
        public int JobId { get; set; }
        public Job Job { get; set; }
        public int ProviderId { get; set; }
        public Provider Provider { get; set; }
    }
}
