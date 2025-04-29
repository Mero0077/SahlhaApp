using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.Models
{
    public enum DocumentStatus
    {
        Pending,
        Approved,
        Rejected
    }

    public class Document
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime VerifiedAt { get; set; }
        public string? CV { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DocumentStatus Status { get; set; } = DocumentStatus.Pending;
        //Navigation Prop 
        public int ProviderId { get; set; }
        public int DocumentTypeId { get; set; }
        public DocumentType DocumentType { get; set; }
        public Provider Provider { get; set; }


    }
}
