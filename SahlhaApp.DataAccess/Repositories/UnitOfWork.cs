using SahlhaApp.DataAccess.Data;
using SahlhaApp.DataAccess.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;
      
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _context = dbContext;
           

        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
