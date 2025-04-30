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
        public decimal? Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public TimeOnly EstimatedTime { get; set; }
        public decimal? PaidAmount { get; set; }
        //Navigation Prop 
        public Task Task { get; set; }
        public int TaskId { get; set; }
        public Provider Provider { get; set; }
        public int ProviderId { get; set; }
    }
}
