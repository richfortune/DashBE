using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DashBe.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        IQueryable<T> GetData<T>(bool trackingChanges = false) where T : class;

        void Insert<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task SaveAsync();

        Task<bool> ExistsAsync<T>(params object[] key) where T : class;
        Task<bool> AnyAsync<T>(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) where T : class;
        Task<T> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) where T : class;

        Task LogAsync(string message, string logLevel, string exception = null);

    }
}
