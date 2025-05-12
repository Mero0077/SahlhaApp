using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.Models
{
    public enum Status
    {
        Pending,
        Approved,
        Rejected
    }
    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public DateTime UploadedAt { get; set; }
        public Status Status { get; set; } = Status.Pending;
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}