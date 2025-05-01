using SahlhaApp.DataAccess.Data;
using SahlhaApp.DataAccess.Repositories.IRepositories;
using SahlhaApp.Models.Models.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.DataAccess.Repositories
{
    public class ProviderRepositry : Repository<Provider>, IProviderRepository
    {
        public ProviderRepositry(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
