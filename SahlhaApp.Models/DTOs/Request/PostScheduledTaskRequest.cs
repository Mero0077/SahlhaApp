using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.DTOs.Request
{
    public class PostScheduledTaskRequest:PostJobRequest
    {
        public DateTime ScheduledFor { get; set; }
        public bool IsRecurring { get; set; }
    }
}
