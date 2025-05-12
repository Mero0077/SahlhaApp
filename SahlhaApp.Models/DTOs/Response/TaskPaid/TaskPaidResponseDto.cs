using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.DTOs.Response.TaskPaid
{
    public class TaskPaidResponseDto
    {
        public string ImageUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Rate { get; set; }
        public decimal Price { get; set; }
    }
}
