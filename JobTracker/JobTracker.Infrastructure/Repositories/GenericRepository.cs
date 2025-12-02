
using System.Linq.Expressions;
using JobTracker.Application.Interfaces;
using JobTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Infrastructure.Repositories
{
    public class GenericRepository<T>(ApplicationDbContext context) : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context = context;
        public async Task<T?> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            IReadOnlyList<T> result = await _context.Set<T>().ToListAsync();
            return result;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            T? entity = await _context.Set<T>().FindAsync(id);
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            /*context.Entry(entity).State = EntityState.Modified;*/
            await context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            IReadOnlyList<T> result = await _context.Set<T>().Where(predicate).ToListAsync();
            return result;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AnyAsync(predicate);
        }

        public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            // This generates "SELECT TOP 1 * ..." SQL query
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }
    }
}
