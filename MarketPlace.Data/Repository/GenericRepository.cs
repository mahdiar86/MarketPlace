using MarketPlace.Data.Context;
using MarketPlace.Data.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Data.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly MarketPlaceDbContext _context;
        private readonly DbSet<TEntity> _dbset;

        public GenericRepository(MarketPlaceDbContext context)
        {
            _context = context;
            this._dbset = _context.Set<TEntity>();
        }

        public async Task AddEntity(TEntity entity)
        {
            entity.CreateDate = DateTime.Now;
            entity.LastUpdateDate = entity.CreateDate;
            await _dbset.AddAsync(entity);
        }

        public void DeleteEntity(TEntity entity)
        {
            entity.IsDelete = true;
            EditEntity(entity);
        }

        public async Task DeleteEntity(long entityId)
        {
            TEntity entity = await GetEntityById(entityId);
            if (entity != null) DeleteEntity(entity);
        }

        public void DeletePermanent(TEntity entity)
        {
            _dbset.Remove(entity);
        }

        public async Task DeletePermanent(long entityId)
        {
            TEntity entity = await GetEntityById(entityId);
            if (entity != null)
                DeletePermanent(entity);
        }

        public async Task AddRangeEntities(List<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                await AddEntity(entity);
            }
        }

        public void EditEntity(TEntity entity)
        {
            entity.LastUpdateDate = DateTime.Now;
            _dbset.Update(entity);
        }

        public async Task<TEntity> GetEntityById(long entityId)
        {
            return await _dbset.SingleOrDefaultAsync(m => m.Id == entityId && !m.IsDelete);
        }

        public IQueryable<TEntity> GetQuery()
        {
            return _dbset.AsQueryable();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (_context != null)
            {
                await _context.DisposeAsync();
            }
        }
    }
}
