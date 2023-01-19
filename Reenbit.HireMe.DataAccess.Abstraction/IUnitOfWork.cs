using Reenbit.HireMe.DataAccess.Abstraction.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reenbit.HireMe.DataAccess.Abstraction
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        T GetRepository<T>()
            where T : class, IBaseRepository;
    }
}
