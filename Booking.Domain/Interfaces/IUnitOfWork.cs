using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Booking.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> SaveChangeAsync();

        Task BeginTransaction();

        Task<bool> CommitTransaction();

        Task RollbackTransaction();
    }
}
