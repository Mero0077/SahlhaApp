using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SahlhaApp.Models.Models;
using SahlhaApp.Models.Models.Documents;
using SahlhaApp.Models.Models.Payments;
using SahlhaApp.Models.Models.Providers;
using SahlhaApp.Models.Models.Services;
using SahlhaApp.Models.Models.Tasks;

namespace SahlhaApp.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        //DbSet
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<SubService> SubServices { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<PendingProviderVerification> PendingProviderVerifications { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentMethod> paymentMethods { get; set; }
        public DbSet<MonthlySubscription> MonthlySubscriptions { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Dispute> Disputes { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> documentTypes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Models.Models.Task> Tasks { get; set; }
        public DbSet<TaskBid> TaskBids { get; set; }
        public DbSet<TaskAssignment> TaskAssignments { get; set; }
        public DbSet<ProviderSubService> ProviderSubServices { get; set; }
        public DbSet<ProviderServicesAvailability> ProviderServicesAvailability { get; set; }
    }
}