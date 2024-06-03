using Data.Data;
using Data.Entities;
using Data.Exceptions;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly TradeMarketDbContext _tradeMarketDbContext;
        protected Repository(TradeMarketDbContext tradeMarketDbContext)
        {
            _tradeMarketDbContext = tradeMarketDbContext;
        }
        public async Task AddAsync(TEntity entity)
        {
            await _tradeMarketDbContext.Set<TEntity>().AddAsync(entity);
        }

        public void Delete(TEntity entity)
        {
             _tradeMarketDbContext.Set<TEntity>().Remove(entity);
             _tradeMarketDbContext.SaveChanges();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            _tradeMarketDbContext.Set<TEntity>().Remove(entity);
            await _tradeMarketDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _tradeMarketDbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            var entity = await _tradeMarketDbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                throw new DataBaseException("Object not found!");
            }

            return entity;
        }

        public void Update(TEntity entity)
        {
            _tradeMarketDbContext.Set<TEntity>().Update(entity);
            _tradeMarketDbContext.SaveChanges();
        }
    }
}
