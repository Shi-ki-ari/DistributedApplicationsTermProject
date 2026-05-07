using Microsoft.EntityFrameworkCore;
using StatusPageData;
using StatusPageData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusPageRepo.Implementations
{
    public class BaseImplementation<T> : IRepo<T> where T : BaseEntity
    {

        private readonly AppDbContext _context;
        private readonly DbSet<T> _set;

        public BaseImplementation(AppDbContext context)
        {
            _context = context;
            _set = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            _set.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _set.FindAsync(id);
            if (entity is null)
            {
                return;
            }
            _set.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _set.ToListAsync();
        }

        public async Task<T?> GetById(int id)
        {
            return await _set.FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _set.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
