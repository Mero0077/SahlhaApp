using SahlhaApp.DataAccess.Data;
using SahlhaApp.DataAccess.Repositories.IRepositories;
using SahlhaApp.Models.Models;
using SahlhaApp.Utility.NotifcationService.NotificationEvents;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Utility.NotifcationService
{
    public class JobService
    {
        private readonly IUnitOfWork _unitOfWork;

        // declare event using delegate
        public event OnJobPostedEventHandler JobPostedEvent;
        public event OnBidPlacedEventHandler BidPlacedEvent;
        public event OnTaskAssignedEventHandler TaskAssignedEvent;

        public JobService( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public async Task<TaskAssignment> AddTaskAssignment(TaskAssignment taskAssignment)
        {
            var TaskAssignemntAdded= await _unitOfWork.TaskAssignment.Add(taskAssignment);

            if (TaskAssignedEvent != null)
            {
                TaskAssignedEvent?.Invoke(new OnTaskAssignedEvent(TaskAssignemntAdded));
            }

            return TaskAssignemntAdded;
        }

        public async Task<TaskBid> AddTaskBid(TaskBid bid)
        {
          var BidAdded=  await _unitOfWork.TaskBid.Add(bid);
            if (BidPlacedEvent != null)
            {
                BidPlacedEvent?.Invoke(new OnBidPlacedEvent(bid));
            }

            return BidAdded;
        }

        public async Task<Job> AddJobAsync(Job job)
        {
            // Add job to database
            await _unitOfWork.Job.Add(job);

             if (JobPostedEvent != null)
            {
                 JobPostedEvent?.Invoke(new OnJobPostedEvent(job));
            }

            return job;
        }
    }
}
