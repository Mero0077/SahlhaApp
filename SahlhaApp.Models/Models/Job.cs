using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.Models
{
    public enum JobStatus
    {
        Pending,
        InProgress,
        Completed,
        Cancelled
    }
    public class Job
    {
        public int Id { get; set; }
        public int SubServiceId { get; set; }
        public string Name { get; set; }=string.Empty;
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
        public decimal Duration { get; set; }
        public JobStatus JobStatus { get; set; } = JobStatus.Pending;
        public DateTime? CancelledAt { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public SubService SubService { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<TaskAssignment> TaskAssignments { get; set; } = new List<TaskAssignment>();
    }
}