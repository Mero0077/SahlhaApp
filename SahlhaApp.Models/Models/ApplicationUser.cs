using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Stripe;

namespace SahlhaApp.Models.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? District { get; set; }
        public string? Street { get; set; }
        public string? ZipCode { get; set; }
        public int? BuildingNumber { get; set; }
        public string? Address { get; set; }
        public string? ImgUrl { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        public bool? Gender { get; set; } = false;
        public decimal? LocationLatitude { get; set; }
        public decimal? LocationLongitude { get; set; }
        //Provider Status
        public bool? IsProvider { get; set; } = false;
        //Navigation Prop
        public ICollection<Task>? Tasks { get; set; }
        public Provider Provider { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public PendingProviderVerification PendingProviderVerifications { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public Payment Payment { get; set; }

    }
}
