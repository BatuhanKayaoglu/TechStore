using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EksiSozluk.Api.Application.Cache
{
    public interface IGenericRedisService<T> where T : class
    {
        Task<T> GetByIdAsync(Guid key, CancellationToken cancellationToken);
        Task DeleteAsync(Guid key, CancellationToken cancellationToken);
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken);
        Task SetAsync(T entity, Guid key, CancellationToken cancellationToken);
        Task UpdateAsync(T entity, Guid key, CancellationToken cancellationToken);
        Task<bool> EntityExistsAsync(Guid key, CancellationToken cancellationToken);
    }
}
