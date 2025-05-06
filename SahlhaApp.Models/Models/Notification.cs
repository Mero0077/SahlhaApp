using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.Models
{
    public enum NotificationType
    {
        JobPosted,
        TaskBidded,
        TaskAssigned
    }
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(50)")] // to save it as a string in db instead of 0,1
        public NotificationType Type { get; set; }
        public int ReferenceId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? SentAt { get; set; } = DateTime.Now;
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}