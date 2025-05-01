using SahlhaApp.DataAccess.Data;
using SahlhaApp.DataAccess.Repositories.IRepositories;
using SahlhaApp.Models.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.DataAccess.Repositories
{
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        public ServiceRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
