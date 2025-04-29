using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;
        public DateTime SentAt { get; set; } = DateTime.Now;
        //Navigation Prop 
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

    }
}
