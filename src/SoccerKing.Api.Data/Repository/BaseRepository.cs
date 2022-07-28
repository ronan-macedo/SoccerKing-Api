using Microsoft.EntityFrameworkCore;
using SoccerKing.Api.Data.Context;
using SoccerKing.Api.Domain.Entities;
using SoccerKing.Api.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoccerKing.Api.Data.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly MyDbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public BaseRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                T result = await _dbSet.SingleOrDefaultAsync(p => p.Id.Equals(id));

                if (result == null)
                    return false;

                _dbSet.Remove(result);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ExistAsync(Guid id)
        {
            return await _dbSet.AnyAsync(p => p.Id.Equals(id));
        }

        public async Task<T> InsertAsync(T item)
        {
            try
            {
                if (item.Id == Guid.Empty)
                    item.Id = Guid.NewGuid();

                item.CreateAt = DateTime.UtcNow;

                _dbSet.Add(item);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return null;
            }

            return item;
        }

        public async Task<T> SelectAsync(Guid id)
        {
            try
            {
                return await _dbSet.SingleOrDefaultAsync(p => p.Id.Equals(id));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<T>> SelectAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<T> UpdateAsync(T item)
        {
            try
            {
                T result = await _dbSet.SingleOrDefaultAsync(p => p.Id.Equals(item.Id));

                if (result == null)
                    return null;

                item.UpdateAt = DateTime.UtcNow;
                item.CreateAt = result.CreateAt;

                _dbContext.Entry(result).CurrentValues.SetValues(item);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                return null;
            }

            return item;
        }
    }
}
