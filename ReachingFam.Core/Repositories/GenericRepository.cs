using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReachingFam.Core.Data;
using ReachingFam.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReachingFam.Core.Repositories
{
    public class GenericRepository<T>(ApplicationDbContext context, ILogger<GenericRepository<T>> logger) : IGenericRepository<T> where T : class
    {        
        protected DbSet<T> dbSet = context.Set<T>();
        protected readonly ILogger<GenericRepository<T>> _logger = logger;

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            try
            {
                return await dbSet.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting entity with id {Id}", id);
                return null;
            }
        }

        public async Task<bool> AddAsync(T item)
        {
            try
            {
                await dbSet.AddAsync(item);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding entity");
                return false;
            }
        }

        public bool Update(T item)
        {
            try
            {
                dbSet.Update(item);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding entity");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await dbSet.FindAsync(id);
                if (entity != null)
                {
                    dbSet.Remove(entity);
                    return true;
                }
                else
                {
                    _logger.LogWarning("Entity with id {Id} not found for deletion", id);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting entity with id {Id}", id);
                return false;
            }
        }
    }
}
