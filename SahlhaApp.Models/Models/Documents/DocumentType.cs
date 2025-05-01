using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.Models.Models.Documents
{
    public class DocumentType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Document> Documents { get; set; }
    }
}
