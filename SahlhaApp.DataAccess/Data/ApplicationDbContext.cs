using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SahlhaApp.Models.Models;

namespace SahlhaApp.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        //DbSet
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<ProviderSubServices> ProviderSubServices { get; set; }
        public DbSet<PendingProviderVerification> PendingProviderVerifications { get; set; }
        public DbSet<SubService> SubServices { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ProviderServiceAvailability> ProviderServiceAvailability { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<ScheduledTask> ScheduledTasks { get; set; }
        public DbSet<TaskBid> TaskBids { get; set; }
        public DbSet<TaskAssignment> TaskAssignments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Dispute> Disputes { get; set; }
        public DbSet<Notification> Nofications { get; set; }
        public DbSet<Rate> Rates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // TaskAssignment relation fixes
            modelBuilder.Entity<TaskAssignment>()
                .HasOne(t => t.Job)
                .WithMany(j => j.TaskAssignments)
                .HasForeignKey(t => t.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TaskAssignment>()
                .HasOne(t => t.Provider)
                .WithMany(p => p.TaskAssignments)
                .HasForeignKey(t => t.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);

            // TaskBid relation fix
            modelBuilder.Entity<TaskBid>()
                .HasOne(tb => tb.Provider)
                .WithMany(p => p.TaskBids)
                .HasForeignKey(tb => tb.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rate>()
                .HasOne(r => r.Provider)
                .WithMany(p => p.Rates)
                .HasForeignKey(r => r.ProviderId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Rate>()
                .HasOne(r => r.ApplicationUser)
                .WithMany()
                .HasForeignKey(r => r.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Job>().ToTable("Jobs");
            modelBuilder.Entity<ScheduledTask>().ToTable("ScheduledTasks");
        }


    }
}