using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Threading.Tasks;

namespace TaskTracker.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
