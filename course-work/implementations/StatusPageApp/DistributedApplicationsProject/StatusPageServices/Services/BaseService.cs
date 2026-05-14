
using StatusPageData.Entities;
using StatusPageRepo;
using StatusPageServices.Interfaces;

namespace StatusPageServices.Services
{
    public abstract class BaseService<T> where T : BaseEntity
    {

        protected readonly IRepo<T> _repo;

        public BaseService(IRepo<T> repo)
        {
            _repo = repo;
        }

        public async Task AddEntityAsync(T entity)
        {
            await _repo.AddAsync(entity);

        }

        public async Task DeleteEntityAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllEntityAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<T?> GetEntityByIdAsync(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task UpdateEntityAsync(T entity)
        {
            await _repo.UpdateAsync(entity);
        }
    }
}
