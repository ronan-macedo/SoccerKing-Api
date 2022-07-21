using SoccerKing.Api.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoccerKing.Api.Domain.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistAsync(Guid id);
        Task<T> InsertAsync(T item);
        Task<T> SelectAsync(Guid id);
        Task<IEnumerable<T>> SelectAsync();
        Task<T> UpdateAsync(T item);
    }
}
