using SahlhaApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.DTOs.Request.DisputeRequests
{
    public class DisputeRequestDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Status { get; set; } // false = open, true = closed
        public DateTime? ResolvedAt { get; set; }
        public string ApplicationUserId { get; set; }
        public int TaskAssignmentId { get; set; }
    }
}
