using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.Models
{
    public enum VerificationStatus
    {
        Pending,
        Approved,
        Rejected
    }
    public class PendingProviderVerification
    {
        public int Id { get; set; }
        public DateTime AppliedAt { get; set; } = DateTime.Now;

        public VerificationStatus Status { get; set; } = VerificationStatus.Pending;
        public string? RejectionReason { get; set; }


        // Navigation properties
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
