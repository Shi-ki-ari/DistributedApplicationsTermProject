using StatusPageData.Entities;
using StatusPageRepo;
using StatusPageServices.Interfaces;
using StatusPageServices.RequestDTO;
using StatusPageServices.RequestDTO.Services;
using StatusPageServices.ResponseDTO;
using StatusPageServices.ResponseDTO.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatusPageServices.Services
{
    public class ServicesService : BaseService<StatusPageData.Entities.ServiceEntity>, IServicesService
    {
        public ServicesService(IRepo<StatusPageData.Entities.ServiceEntity> repo) : base(repo)
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
            var e = await GetEntityByIdAsync(id);
            if (e is null) return null;
            return ToDto(e);
        }

        public async Task UpdateAsync(int id, UpdateServiceDto dto)
        {
            var existing = await GetEntityByIdAsync(id);
            if (existing is null) return;

            ApplyUpdate(existing, dto);

            await UpdateEntityAsync(existing);
        }

        private static StatusPageData.Entities.ServiceEntity ToEntity(CreateServiceDto dto)
        {
            return new StatusPageData.Entities.ServiceEntity
            {
                Name = dto.Name,
                TargetUrl = dto.TargetUrl,
                IsOnline = dto.IsOnline,
                DateAdded = System.DateTime.UtcNow,
                UptimePercentage = 0m,
                CategoryId = dto.CategoryId
            };
        }

        private static ServiceDto ToDto(StatusPageData.Entities.ServiceEntity entity)
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

        private static void ApplyUpdate(StatusPageData.Entities.ServiceEntity entity, UpdateServiceDto dto)
        {
            entity.Name = dto.Name;
            entity.TargetUrl = dto.TargetUrl;
            entity.IsOnline = dto.IsOnline;
            entity.CategoryId = dto.CategoryId;
        }


        //pagination and filtering
        public async Task<PagedResult<ServiceDto>> GetPagedServicesAsync(PaginationQuery query)
        {
            var allServices = await _repo.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                allServices = allServices.Where(s =>
                    s.Name.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTargetUrl))
            {
                allServices = allServices.Where(s =>
                    s.TargetUrl.Contains(query.SearchTargetUrl, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                allServices = query.SortBy.ToLower() switch
                {
                    "name" => query.SortDescending ? allServices.OrderByDescending(s => s.Name) : allServices.OrderBy(s => s.Name),
                    "targeturl" => query.SortDescending ? allServices.OrderByDescending(s => s.TargetUrl) : allServices.OrderBy(s => s.TargetUrl),
                    "dateadded" => query.SortDescending ? allServices.OrderByDescending(s => s.DateAdded) : allServices.OrderBy(s => s.DateAdded),
                    "uptime" => query.SortDescending ? allServices.OrderByDescending(s => s.UptimePercentage) : allServices.OrderBy(s => s.UptimePercentage),
                    _ => query.SortDescending ? allServices.OrderByDescending(s => s.Name) : allServices.OrderBy(s => s.Name)
                };
            }
            else
            {
                allServices = query.SortDescending ? allServices.OrderByDescending(s => s.Name) : allServices.OrderBy(s => s.Name);
            }

            int totalCount = allServices.Count();

            var pagedItems = allServices
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(s => ToDto(s))
                .ToList();

            return new PagedResult<ServiceDto>
            {
                Items = pagedItems,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }

    }
}
