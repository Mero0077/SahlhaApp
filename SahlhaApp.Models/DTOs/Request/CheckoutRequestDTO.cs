using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.DTOs.Request
{
    public class CheckoutRequestDTO
    {
        public int TaskAssignmentId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } // "Stripe" or "CashOnDelivery"
        public string Currency { get; set; } = "usd";
    }
}
