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
             //public string? Name { get; set; }
        public int SubServiceId { get; set; }
        public string Description { get; set; }
            //public DateTime CreatedAt { get; set; } = DateTime.Now;
            //public string ApplicationUserId { get; set; }
            //public string Address { get; set; }
            //public decimal Duration { get; set; }
            public JobStatus? jobStatus { get; set; } = JobStatus.Pending;

        public double Latitude { get; set; } 
        public double Longitude { get; set; }
    }

    }

