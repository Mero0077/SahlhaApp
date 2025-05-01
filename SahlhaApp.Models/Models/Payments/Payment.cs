using SahlhaApp.Models.Models.Providers;
using SahlhaApp.Models.Models.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.Models.Payments
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
        public decimal? Amount { get; set; }
        public DateTime PaymenyDate { get; set; } = DateTime.Now;
        public string? PaymentRefrence { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        //Navigation Prop 

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int ProviderId { get; set; }
        public Provider Provider { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
        public int PaymentMethodId { get; set; }
        public TaskAssignment TaskAssignment { get; set; }
        public int TaskAssignmentId { get; set; }




    }
}
