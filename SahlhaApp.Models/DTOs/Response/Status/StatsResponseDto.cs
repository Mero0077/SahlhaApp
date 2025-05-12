using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.DTOs.Response.Status
{
    public class StatsResponseDto
    {
        public int Providers { get; set; }             // Number of Providers (e.g., Doctors)
        public int ServiceCategories { get; set; }     // Number of Service Categories (e.g., Departments)
        public int ActiveServices { get; set; }        // Number of Active Services (e.g., Research Labs)
        public int CustomerReviews { get; set; }
    }
}
