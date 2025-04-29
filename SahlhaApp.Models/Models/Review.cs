using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SahlhaApp.Models.Models
{
    [PrimaryKey(nameof(ApplicationUserId), nameof(ProviderId))]
    public class Review
    {
        public Decimal? Rating { get; set; }
        public String? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        //Navigation Prop 
        public String ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int ProviderId { get; set; }
        public Provider Provider { get; set; }
    }
}
