using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StatusPageData.Entities;
using StatusPageRepo;
using StatusPageServices.Interfaces;
using StatusPageServices.RequestDTO.Services;
using StatusPageServices.ResponseDTO.Services;

namespace StatusPageServices.Services
{
    public class ServicesService : BaseService<StatusPageData.Entities.Services>, IServicesService
    {
        public ServicesService(IRepo<StatusPageData.Entities.Services> repo) : base(repo)
        {
        }

        public async Task<ServiceDto> CreateAsync(CreateServiceDto dto)
        {
            var e = ToEntity(dto);

            await AddEntityAsync(e);

            return ToDto(e);
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteEntityAsync(id);
        }

        public async Task<IEnumerable<ServiceDto>> GetAllAsync()
        {
            var entities = await GetAllEntityAsync();
            return entities.Select(ToDto);
        }

        public async Task<ServiceDto?> GetByIdAsync(int id)
        {
            var e = await GetEntityById(id);
            if (e is null) return null;
            return ToDto(e);
        }

        public async Task UpdateAsync(int id, UpdateServiceDto dto)
        {
            var existing = await GetEntityById(id);
            if (existing is null) return;

            ApplyUpdate(existing, dto);

            await UpdateEntityAsync(existing);
        }

        private static StatusPageData.Entities.Services ToEntity(CreateServiceDto dto)
        {
            return new StatusPageData.Entities.Services
            {
                Name = dto.Name,
                TargetUrl = dto.TargetUrl,
                IsOnline = dto.IsOnline,
                DateAdded = System.DateTime.UtcNow,
                UptimePercentage = 0m,
                CategoryId = dto.CategoryId
            };
        }

        private static ServiceDto ToDto(StatusPageData.Entities.Services entity)
        {
            return new ServiceDto(
                entity.Id,
                entity.Name,
                entity.TargetUrl,
                entity.IsOnline,
                entity.DateAdded,
                entity.UptimePercentage,
                entity.CategoryId
            );
        }

        private static void ApplyUpdate(StatusPageData.Entities.Services entity, UpdateServiceDto dto)
        {
            entity.Name = dto.Name;
            entity.TargetUrl = dto.TargetUrl;
            entity.IsOnline = dto.IsOnline;
            entity.CategoryId = dto.CategoryId;
        }

    }
}
