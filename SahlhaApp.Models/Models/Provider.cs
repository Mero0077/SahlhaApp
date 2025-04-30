using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stripe;

namespace SahlhaApp.Models.Models
{
    public class Provider 
    {
        public int Id { get; set; }
        public string? VerificationLevel { get; set; }
        public bool? VerificationStatus { get; set; }
        public int? CompletedTasks { get; set; }
        public int? TotalTasks { get; set; }
        public string? Brief { get; set; }
        public string[]? Skills { get; set; }
        public decimal? HourlyRate { get; set; }
        public TimeOnly AvailableFrom { get; set; }
        public TimeOnly AvailableTo { get; set; }
        //Navigation Prop 
        public String ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<MonthlySubscription> MonthlySubscriptions { get; set; }
        public ICollection<Document> Documents { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<SubService> SubServices { get; set; }
        public ICollection<Rate> Rates { get; set; }
        public ICollection<ProviderServicesAvailability> ProviderServicesAvailability { get; set; }
        public TaskBid TaskBid { get; set; }
        public TaskAssignment TaskAssignment { get; set; }
        public Payment Payment { get; set; }


    }
}
