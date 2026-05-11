using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StatusPageData.Entities;
using StatusPageRepo;
using StatusPageServices.Interfaces;
using StatusPageServices.RequestDTO.ServiceCategories;
using StatusPageServices.ResponseDTO.ServiceCategories;

namespace StatusPageServices.Services
{
    public class ServiceCategoriesService : BaseService<ServiceCategories>, IServiceCategoriesService
    {
        public ServiceCategoriesService(IRepo<ServiceCategories> repo) : base(repo)
        {
        }

        public async Task<ServiceCategoryDto> CreateAsync(CreateServiceCategoryDto dto)
        {
            var e = new ServiceCategories
            {
                Name = dto.Name,
                Description = dto.Description,
                DisplayOrder = dto.DisplayOrder,
                CreatedAt = System.DateTime.UtcNow,
                Notify = dto.Notify
            };

            await AddEntityAsync(e);

            return new ServiceCategoryDto(
                e.Id,
                e.Name,
                e.Description,
                e.DisplayOrder,
                e.CreatedAt,
                e.Notify
            );
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteEntityAsync(id);
        }

        public async Task<IEnumerable<ServiceCategoryDto>> GetAllAsync()
        {
            var entities = await GetAllEntityAsync();
            return entities.Select(e => new ServiceCategoryDto(
                e.Id,
                e.Name,
                e.Description,
                e.DisplayOrder,
                e.CreatedAt,
                e.Notify
            ));
        }

        public async Task<ServiceCategoryDto?> GetByIdAsync(int id)
        {
            var e = await GetEntityById(id);
            if (e is null) return null;
            return new ServiceCategoryDto(e.Id, e.Name, e.Description, e.DisplayOrder, e.CreatedAt, e.Notify);
        }

        public async Task UpdateAsync(int id, UpdateServiceCategoryDto dto)
        {
            var existing = await GetEntityById(id);
            if (existing is null) return;

            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.DisplayOrder = dto.DisplayOrder;
            existing.Notify = dto.Notify;

            await UpdateEntityAsync(existing);
        }

    }
}
