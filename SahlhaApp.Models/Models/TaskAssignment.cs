using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.Models
{
    public class TaskAssignment
    {
        public int Id { get; set; }
        public bool IsAccepted { get; set; } = false;
        public decimal FinalPrice { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.Now;
        public bool IsCompleted { get; set; } = false;
        public int JobId { get; set; }
        public Job Job { get; set; }

        public int ProviderId { get; set; }
        public Provider Provider { get; set; }
        public Payment Payment { get; set; }
        public Dispute Dispute { get; set; }
    }
}