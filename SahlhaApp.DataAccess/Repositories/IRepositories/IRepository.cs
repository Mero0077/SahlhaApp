using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SahlhaApp.DataAccess.Repositories.IRepositories
{
    public interface IRepository<T>
    {
        public Task<T> Add(T entity, CancellationToken cancellationToken = default);
        public Task<IEnumerable<T>> AddAll(List<T> entities, CancellationToken cancellationToken = default);
        public Task<T> Edit(T entity, CancellationToken cancellationToken = default);
        public Task<T> Delete(T entity, CancellationToken cancellationToken = default);
        public Task<IEnumerable<T>> DeleteAll(List<T> entities, CancellationToken cancellationToken = default);
        public Task<bool> Commit();

        public IQueryable<T> GetAll(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true);

        public Task<T>? GetOne(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true);

        public Task<bool> Exists(Expression<Func<T, bool>> filter);
    }
}
