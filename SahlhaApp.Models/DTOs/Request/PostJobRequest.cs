using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.DTOs.Request
{
    public class PostJobRequest
    {
   
        public enum JobStatus
        {
            Pending,
            InProgress,
            Completed,
            Cancelled
        }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int SubServiceId { get; set; }
    }

    }

