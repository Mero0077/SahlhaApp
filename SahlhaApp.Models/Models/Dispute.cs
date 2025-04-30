using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.Models
{

    public class Dispute
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool? Status { get; set; } = false;
        public DateTime? ResolvedAt { get; set; }
        //Navigation Prop
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }



    }
}
