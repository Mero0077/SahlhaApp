using Microsoft.AspNetCore.Mvc.Formatters;
using SahlhaApp.DataAccess.Data;
using SahlhaApp.DataAccess.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;
        public IDisputeRepository Dispute { get; private set; }
        public IDocumentRepository Document{ get; private set; }
        public IDocumentTypeRepository DocumentType{ get; private set; }
        public IJobRepository Job{ get; private set; }
        public INotificationRepository Notification{ get; private set; }
        public IPaymentRepository Payment{ get; private set; }
        public IPaymentMethodRepository PaymentMethod{ get; private set; }
        public IPendingProviderVerificationRepository PendingProviderVerification{ get; private set; }
        public IProviderRepository Provider{ get; private set; }
        public IProviderServiceAvailabilityRepository ProviderServiceAvailability{ get; private set; }
        public IProviderSubServicesRepository ProviderSubServices{ get; private set; }
        public IRateRepository Rate{ get; private set; }
        public IServiceRepository Service{ get; private set; }
        public ISubscriptionRepository Subscription{ get; private set; }
        public ISubServiceRepositry SubService{ get; private set; }
        public ITaskAssignmentRepository TaskAssignment{ get; private set; }
        public ITaskBidRepository TaskBid{ get; private set; }
        public IUserRepository User{ get; private set; }
        public IScheduledTask ScheduledTask{ get; private set; }
        public IContactUsMessageRepository ContactUsMessage{ get; private set; }
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _context = dbContext;
            Dispute= new DisputeRepository(dbContext);
            Document= new DocumentRepository(dbContext);
            DocumentType= new DocumentTypeRepository(dbContext);
            Job= new JobRepository(dbContext);
            Notification= new NoficationRepository(dbContext);
            Payment= new PaymentRepository(dbContext);
            PaymentMethod= new PaymentMethodRepository(dbContext);
            PendingProviderVerification = new PendingProviderVerificationRepository(dbContext);
            Provider= new ProviderRepositry(dbContext);
            ProviderServiceAvailability= new ProviderServiceAvailabilityRepository(dbContext);
            ProviderSubServices= new ProviderSubServicesRepository(dbContext);
            Rate= new RateRepository(dbContext);
            Service= new ServiceRepository(dbContext);
            Subscription= new SubscriptionRepository(dbContext);
            SubService= new SubServiceRepository(dbContext);
            TaskAssignment= new TaskAssignmentRepository(dbContext);
            TaskBid= new TaskBidRepository(dbContext);
            User= new UserRepository(dbContext);
            ScheduledTask= new ScheduledTaskRepository(dbContext);
            ContactUsMessage= new ContactUsMessageRepository(dbContext);
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
