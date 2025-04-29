using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.Models
{
    public class DocumentType
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        //Navigation Prop 
        public ICollection<Document> Documents { get; set; }

    }
}
