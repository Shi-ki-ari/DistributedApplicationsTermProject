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
    public class ServiceCategoriesService : BaseService<ServiceCategoryEntity>, IServiceCategoriesService
    {
        public ServiceCategoriesService(IRepo<ServiceCategoryEntity> repo) : base(repo)
        {
        }

        public async Task<ServiceCategoryDto> CreateAsync(CreateServiceCategoryDto dto)
        {
            var e = ToEntity(dto);

            await AddEntityAsync(e);

            return ToDto(e);
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteEntityAsync(id);
        }

        public async Task<IEnumerable<ServiceCategoryDto>> GetAllAsync()
        {
            var entities = await GetAllEntityAsync();
            return entities.Select(ToDto);
        }

        public async Task<ServiceCategoryDto?> GetByIdAsync(int id)
        {
            var e = await GetEntityByIdAsync(id);
            if (e is null) return null;
            return ToDto(e);
        }

        public async Task UpdateAsync(int id, UpdateServiceCategoryDto dto)
        {
            var existing = await GetEntityByIdAsync(id);
            if (existing is null) return;

            ApplyUpdate(existing, dto);

            await UpdateEntityAsync(existing);
        }

        private static ServiceCategoryEntity ToEntity(CreateServiceCategoryDto dto)
        {
            return new ServiceCategoryEntity
            {
                Name = dto.Name,
                Description = dto.Description,
                DisplayOrder = dto.DisplayOrder,
                CreatedAt = System.DateTime.UtcNow,
                Notify = dto.Notify
            };
        }

        private static ServiceCategoryDto ToDto(ServiceCategoryEntity entity)
        {
            return new ServiceCategoryDto(
                entity.Id,
                entity.Name,
                entity.Description,
                entity.DisplayOrder,
                entity.CreatedAt,
                entity.Notify
            );
        }

        private static void ApplyUpdate(ServiceCategoryEntity entity, UpdateServiceCategoryDto dto)
        {
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.DisplayOrder = dto.DisplayOrder;
            entity.Notify = dto.Notify;
        }

    }
}
