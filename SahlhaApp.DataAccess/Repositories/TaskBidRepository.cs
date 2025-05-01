using SahlhaApp.DataAccess.Data;
using SahlhaApp.DataAccess.Repositories.IRepositories;
using SahlhaApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.DataAccess.Repositories
{
    public class TaskBidRepository : Repository<TaskBid>, ITaskBidRepository
    {
        public TaskBidRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
