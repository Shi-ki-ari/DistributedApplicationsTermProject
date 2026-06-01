using StatusPageData.Entities;
using StatusPageRepo;
using StatusPageServices.Interfaces;
using StatusPageServices.RequestDTO;
using StatusPageServices.RequestDTO.Engineers;
using StatusPageServices.ResponseDTO;
using StatusPageServices.ResponseDTO.Engineers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatusPageServices.Services
{
    public class EngineersService : BaseService<EngineerEntity>, IEngineersService
    {
        public EngineersService(IRepo<EngineerEntity> repo) : base(repo)
        {
        }

        public async Task<EngineerDto> CreateAsync(CreateEngineerDto dto)
        {
            var e = ToEntity(dto);

            await AddEntityAsync(e);

            return ToDto(e);
        }

        public async Task<IEnumerable<EngineerDto>> GetAllAsync()
        {
            var entities = await GetAllEntityAsync();
            return entities.Select(ToDto);
        }

        public async Task<EngineerDto?> GetByIdAsync(int id)
        {
            var e = await GetEntityByIdAsync(id);
            if (e is null) return null;
            return ToDto(e);
        }

        public async Task UpdateAsync(int id, UpdateEngineerDto dto)
        {
            var existing = await GetEntityByIdAsync(id);
            if (existing is null) return;

            ApplyUpdate(existing, dto);

            await UpdateEntityAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteEntityAsync(id);
        }

        private static EngineerEntity ToEntity(CreateEngineerDto dto)
        {
            return new EngineerEntity
            {
                Name = dto.Name,
                Email = dto.Email,
                HiredDate = dto.HiredDate ?? System.DateTime.UtcNow,
                OnCall = dto.OnCall,
                HourlyRate = dto.HourlyRate
            };
        }

        private static EngineerDto ToDto(EngineerEntity entity)
        {
            return new EngineerDto(entity.Id, entity.Name, entity.Email, entity.HiredDate, entity.OnCall, entity.HourlyRate);
        }

        private static void ApplyUpdate(EngineerEntity entity, UpdateEngineerDto dto)
        {
            entity.Name = dto.Name;
            entity.Email = dto.Email;
            entity.HiredDate = dto.HiredDate;
            entity.OnCall = dto.OnCall;
            entity.HourlyRate = dto.HourlyRate;
        }

        //filtering, sorting and pagination
        public async Task<PagedResult<EngineerDto>> GetPagedEngineersAsync(PaginationQuery query)
        {
            //filtering
            var allEngineers = await _repo.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                allEngineers = allEngineers.Where(e =>
                    e.Name.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(query.SearchEmail))
            {
                allEngineers = allEngineers.Where(e =>
                    e.Email.Contains(query.SearchEmail, StringComparison.OrdinalIgnoreCase));
            }
            //sorting
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                allEngineers = query.SortBy.ToLower() switch
                {
                    "name" => query.SortDescending ? allEngineers.OrderByDescending(e => e.Name) : allEngineers.OrderBy(e => e.Name),
                    "email" => query.SortDescending ? allEngineers.OrderByDescending(e => e.Email) : allEngineers.OrderBy(e => e.Email),
                    "hireddate" => query.SortDescending ? allEngineers.OrderByDescending(e => e.HiredDate) : allEngineers.OrderBy(e => e.HiredDate),
                    "hourlyrate" => query.SortDescending ? allEngineers.OrderByDescending(e => e.HourlyRate) : allEngineers.OrderBy(e => e.HourlyRate),
                    _ => query.SortDescending ? allEngineers.OrderByDescending(e => e.Name) : allEngineers.OrderBy(e => e.Name)
                };
            }
            else
            {
                allEngineers = query.SortDescending ? allEngineers.OrderByDescending(e => e.Name) : allEngineers.OrderBy(e => e.Name);
            }
            //pagination
            int totalCount = allEngineers.Count();

            var pagedItems = allEngineers
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(e => ToDto(e))
                .ToList();

            return new PagedResult<EngineerDto>
            {
                Items = pagedItems,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }
    }

}
