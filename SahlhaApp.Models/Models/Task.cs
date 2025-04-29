using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.Models
{
    public enum TaskDuration
    {
        Small,
        Medium,
        Large

    }
    public enum TaskStatus
    {
        Pending,
        Accepted,
        Rejected,
        Completed
    }

    public class Task
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.Pending;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? Location { get; set; }
        public TaskDuration DurationTime { get; set; } = TaskDuration.Small;
        //Navigation Prop 
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public TaskAssignment TaskAssignment { get; set; }
        public ICollection<TaskBid> TaskBids { get; set; }


    }
}
