using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.Models
{
    public class Provider
    {
        public int Id { get; set; }
        public string VerificationLevel { get; set; } = "Unverified";
        public int CompletedTasks { get; set; } = 0;
        public string Description { get; set; }
        public decimal HourlyRate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<Subscription> Subscription { get; set; }
        public ICollection<ProviderSubServices> ProviderServices { get; set; }
        public ICollection<ProviderServiceAvailability> ProviderServiceAvailability { get; set; }
        public ICollection<Document> Documents { get; set; }
        public ICollection<TaskBid> TaskBids { get; set; }
        public ICollection<TaskAssignment> TaskAssignments { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<Rate> Rates { get; set; }
    }

}