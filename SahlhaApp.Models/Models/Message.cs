using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.Models
{
    public class Message
    {
        public int Id { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime SentAt { get; set; } = DateTime.Now;
        public string? Description { get; set; }
    }
}
