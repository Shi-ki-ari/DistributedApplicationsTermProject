
using StatusPageData.Entities;
using StatusPageRepo;
using StatusPageServices.Interfaces;

namespace StatusPageServices.Services
{
    public abstract class BaseService<T> : IService<T> where T : BaseEntity
    {

        private readonly IRepo<T> _repo;

        public BaseService(IRepo<T> repo)
        {
            _repo = repo;
        }

        public async Task AddAsync(T entity)
        {
            await _repo.AddAsync(entity);

        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<T?> GetById(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task UpdateAsync(T entity)
        {
            await _repo.UpdateAsync(entity);
        }
    }
}
