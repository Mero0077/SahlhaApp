using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SahlhaApp.DataAccess.Data;
using SahlhaApp.DataAccess.Repositories.IRepositories;
using SahlhaApp.Models.Models;
using SahlhaApp.Utility.NotifcationService.NotificationEvents;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace SahlhaApp.Utility.NotifcationService
{
    public class JobPostedNotificationHandler
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<JobHub> _hubContext;


        public JobPostedNotificationHandler(ApplicationDbContext context, IUnitOfWork unitOfWork, IHubContext<JobHub> hubContext)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        //subscribe to event
        public void Subscribe (JobService jobService)
        {
            jobService.JobPostedEvent += OnJobPosted;

        }

        public void SubscribeTaskBid (JobService jobService)
        {
                jobService.BidPlacedEvent += OnBidPlaced;
           
        }


        public async Task SubscribeTaskAssignment(JobService jobService)
        {
            jobService.TaskAssignedEvent += OnTaskAssigned;
        }



        public async Task OnTaskAssigned(OnTaskAssignedEvent onTaskAssignedEvent)
        {
            var TaskAssigned= onTaskAssignedEvent.TaskAssignment;
            Console.WriteLine($"Task assigned: {TaskAssigned.Id}. Sending notifications...");

            var GetTaskAssigned = await _unitOfWork.TaskAssignment.GetAll(e => e.Id == TaskAssigned.Id).Select(e => new
            {
                e.Id,
                e.FinalPrice,
                e.ProviderId,
                ProviderName = e.Provider.ApplicationUser.FirstName + " " + e.Provider.ApplicationUser.LastName,
                ReceiverId= e.Job.ApplicationUserId
            }).FirstOrDefaultAsync();

            if (GetTaskAssigned == null)
            {
                Console.WriteLine("No task assignment found.");
                return;
            }

            var notification = new Notification()
            {
                Title = "New Task Assigned!",
                Type = NotificationType.TaskAssigned,
                ReferenceId = TaskAssigned.Id,
                Message = $"Your Task is assigned for: {TaskAssigned.FinalPrice} Pounds, your Provider is: {GetTaskAssigned.ProviderName}",
                CreatedAt = DateTime.Now,
                ApplicationUserId = GetTaskAssigned.ReceiverId,
                IsRead = false
            };

            try
            {
                await _unitOfWork.Notification.Add(notification);

                // SignalR: Send to the user who owns the job
                await _hubContext.Clients.User(GetTaskAssigned.ReceiverId).SendAsync("ReceiveNotification", new
                {
                    Title = notification.Title,
                    Message = notification.Message,
                    Type = notification.Type.ToString(),
                    ReferenceId = notification.ReferenceId
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while sending notification: {ex.Message}");
            }
        }
        public async Task OnBidPlaced(OnBidPlacedEvent onBidPlacedEvent)
        {
            var bid = onBidPlacedEvent.TaskBid;
            Console.WriteLine($"Bid posted: {bid.Description}. Sending notifications...");

            //   var GetBid = await _unitOfWork.TaskBid.GetOne(e => e.Id == bid.Id, includes: [e=>e.Job,e=>e.Provider, e => e.Provider.ApplicationUser]); not optiized we getting all properties
            var GetBid = await _unitOfWork.TaskBid.GetAll(e => e.Id == bid.Id).Select(e => new
            {
                e.Id,
                e.Amount,
                JobOwnerId=e.Job.ApplicationUserId,
                BidderName= e.Provider.ApplicationUser.FirstName+" "+ e.Provider.ApplicationUser.LastName
            }).FirstOrDefaultAsync();

            if (GetBid == null) return;

            var notification = new Notification()
            {
                Title = "New Bid Available!",
                Type = NotificationType.TaskBidded  ,
                ReferenceId = bid.Id,
                Message = $"A new Bid for: {bid.Amount} Pounds from{GetBid.BidderName}",
                CreatedAt = DateTime.Now,
                ApplicationUserId = GetBid.JobOwnerId,
                IsRead = false
            };

            try
            {
                await _unitOfWork.Notification.Add(notification);

                // SignalR: Send to the user who owns the job
                await _hubContext.Clients.User(GetBid.JobOwnerId).SendAsync("ReceiveNotification", new
                {
                    Title = notification.Title,
                    Message = notification.Message,
                    Type = notification.Type.ToString(),
                    ReferenceId = notification.ReferenceId
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while sending notification: {ex.Message}");
            }
        }


        public async Task OnJobPosted(OnJobPostedEvent jobPostedEvent)
        {
           var job = jobPostedEvent.Job;
            Console.WriteLine($"Job posted: {job.Description}. Sending notifications...");


            // can be optimized iwt linq to remove .tolist() and no for loop.
            var AllProviders = _unitOfWork.Provider.GetAll(includes: [p => p.ApplicationUser,p=> p.ProviderServices]).ToList();

            var SelectedProviders = new List<Provider>();
            var addedProviderIds = new HashSet<int>();

            foreach (var provider in AllProviders)
            {
                if (provider.ProviderServices != null && provider.ProviderServices.Any(ps => ps.SubServiceId == job.SubServiceId))
                {
                    if (addedProviderIds.Add(provider.Id)) 
                    {
                        SelectedProviders.Add(provider);
                    }
                }
            }
            //////////////////////////////////////////////////////////////////////////////////////
            

            Console.WriteLine("Sending notification for Job ID: " + job.Id);

            foreach (var provider in SelectedProviders)
            {
                Console.WriteLine($"Notifying provider: {provider.Id}");

                var userId = provider.ApplicationUserId;

                // Create a notification
                var notification = new Notification
                {
                    Title = "New Job Available!",
                    Type = NotificationType.JobPosted,
                    ReferenceId = job.Id,
                    Message = $"A new job matches your expertise: {job.Description}",
                    CreatedAt = DateTime.Now,
                    ApplicationUserId = provider.ApplicationUserId,
                    IsRead = false
                };

                try
                {
                    await _unitOfWork.Notification.Add(notification);

                    await _hubContext.Clients.User(userId).SendAsync("ReceiveJobNotification", new
                    {
                        JobId = job.Id,
                        job.Name,
                        job.Description,
                        job.SubServiceId,
                        job.CreatedAt
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occurred while sending notification: {ex.Message}");
                }
               
               
            }
        }
    }
}
