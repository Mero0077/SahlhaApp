using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.Models.Providers
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
        public DateTime appliedAt { get; set; } = DateTime.Now;
        public VerificationStatus VerificationStatus { get; set; } = VerificationStatus.Pending;
        public string? RejectionReason { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
