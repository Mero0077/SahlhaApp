using SahlhaApp.DataAccess.Repositories.IRepositories;
using SahlhaApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Utility
{
    public class CancelJob
    {
        private readonly IUnitOfWork _unitOfWork;

        public CancelJob(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> IsJobCancellable(int jobId, string userId)
        {
            var job = await _unitOfWork.Job.GetOne(j => j.Id == jobId && j.ApplicationUserId == userId);
            return job != null && job.JobStatus != JobStatus.Cancelled && job.JobStatus != JobStatus.Completed;
        }

    }
}
