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
            var e = new StatusPageData.Entities.Services
            {
                Name = dto.Name,
                TargetUrl = dto.TargetUrl,
                IsOnline = dto.IsOnline,
                DateAdded = System.DateTime.UtcNow,
                UptimePercentage = 0m,
                CategoryId = dto.CategoryId
            };

            await AddEntityAsync(e);

            return new ServiceDto(
                e.Id,
                e.Name,
                e.TargetUrl,
                e.IsOnline,
                e.DateAdded,
                e.UptimePercentage,
                e.CategoryId
            );
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteEntityAsync(id);
        }

        public async Task<IEnumerable<ServiceDto>> GetAllAsync()
        {
            var entities = await GetAllEntityAsync();
            return entities.Select(e => new ServiceDto(
                e.Id,
                e.Name,
                e.TargetUrl,
                e.IsOnline,
                e.DateAdded,
                e.UptimePercentage,
                e.CategoryId
            ));
        }

        public async Task<ServiceDto?> GetByIdAsync(int id)
        {
            var e = await GetEntityById(id);
            if (e is null) return null;
            return new ServiceDto(e.Id, e.Name, e.TargetUrl, e.IsOnline, e.DateAdded, e.UptimePercentage, e.CategoryId);
        }

        public async Task UpdateAsync(int id, UpdateServiceDto dto)
        {
            var existing = await GetEntityById(id);
            if (existing is null) return;

            existing.Name = dto.Name;
            existing.TargetUrl = dto.TargetUrl;
            existing.IsOnline = dto.IsOnline;
            existing.CategoryId = dto.CategoryId;

            await UpdateEntityAsync(existing);
        }

    }
}
