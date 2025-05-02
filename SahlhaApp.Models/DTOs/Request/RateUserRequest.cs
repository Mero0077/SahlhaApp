using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.DTOs.Request
{
        public class RateUserRequest
        {
    
                public string RatedUserId { get; set; }
                public int RateValue { get; set; }
                public string Comment { get; set; }

                public DateTime CreatedAt { get; set; }= DateTime.Now;
            }


    }
