using SahlhaApp.DataAccess.Data;
using SahlhaApp.DataAccess.Repositories.IRepositories;
using SahlhaApp.Models.Models.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.DataAccess.Repositories
{
    public class DocumentRepository : Repository<Document>, IDocumentRepository
    {
        public DocumentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
