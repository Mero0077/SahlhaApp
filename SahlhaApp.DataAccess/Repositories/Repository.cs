using Microsoft.EntityFrameworkCore;
using SahlhaApp.DataAccess.Data;
using SahlhaApp.DataAccess.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public DbSet<T> dbSet;
        private readonly ApplicationDbContext _dbContext;
        public Repository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
            dbSet = _dbContext.Set<T>();
        }
        public async Task<T> Add(T entity, CancellationToken cancellationToken = default)
        {
            await dbSet.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return entity;
        }
        public async Task<IEnumerable<T>> AddAll(List<T> entities, CancellationToken cancellationToken = default)
        {
            await dbSet.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return entities;
        }
        public async Task<T> Edit(T entity, CancellationToken cancellationToken = default)
        {
            dbSet.Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return entity;
        }
        public async Task<T> Delete(T entity, CancellationToken cancellationToken = default)
        {
            dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return entity;
        }
        public async Task<IEnumerable<T>> DeleteAll(List<T> entities, CancellationToken cancellationToken = default)
        {
            dbSet.RemoveRange(entities);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return entities;
        }
        public async Task<bool> Commit()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }

            catch (Exception ex)
            {
                Console.WriteLine($"error{ex.Message}");
                return false;
            }
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }


            if (includes != null)
            {
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            }




            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

            public async Task<T?> GetOne(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true)
            {
                return await GetAll(filter, includes, tracked).FirstOrDefaultAsync();
            }

        public async Task<bool> Exists(Expression<Func<T, bool>> filter)
        {
            return await dbSet.AnyAsync(filter);
        }
    }
}
