using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.DataAccess.Repositories.IRepositories
{
    public interface IUnitOfWork
    {
        IProviderRepository Provider { get; }
        IDocumentRepository Document { get; }
        IDocumentTypeRepository DocumentType { get; } 
        IJobRepository Job { get; }
        INotificationRepository Notification { get; }
        IPaymentRepository Payment { get; }
        IPaymentMethodRepository PaymentMethod { get; }
        IPendingProviderVerificationRepository PendingProviderVerification { get; }
        IProviderServiceAvailabilityRepository ProviderServiceAvailability { get; }
        IProviderSubServicesRepository ProviderSubServices { get; }
        IRateRepository Rate { get; }
        IServiceRepository Service{ get; }
        ISubServiceRepositry SubService { get; }
        ITaskAssignmentRepository TaskAssignment { get; }
        ITaskBidRepository TaskBid { get; }
        IUserRepository User { get; }
        IDisputeRepository Dispute { get; } 
        ISubscriptionRepository Subscription { get; } 
        IContactUsMessageRepository ContactUsMessage { get; }
        IScheduledTask ScheduledTask { get; }
        public Task Commit();
    }
}
    