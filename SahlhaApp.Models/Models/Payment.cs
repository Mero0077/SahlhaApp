using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.Models
{
    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed
    }
    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public string TransactionId { get; set; }
        public string PaymentReference { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        // we don't know if we will add provider id
        public int TaskAssignmentId { get; set; }
        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        public TaskAssignment TaskAssignment { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
