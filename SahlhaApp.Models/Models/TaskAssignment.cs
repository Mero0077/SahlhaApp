using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.Models
{

    public class TaskAssignment
    {
        public int Id { get; set; }
        public decimal? Price { get; set; }
        //Navigation Prop 
        public Task Task { get; set; }
        public int TaskId { get; set; }
        public Payment Payment { get; set; }

    }
}
