using StatusPageData.Entities;
using StatusPageRepo;
using StatusPageServices.Interfaces;
using StatusPageServices.RequestDTO;
using StatusPageServices.RequestDTO.Incidents;
using StatusPageServices.ResponseDTO;
using StatusPageServices.ResponseDTO.Incidents;
using System.Linq;
namespace StatusPageServices.Services
{
    public class IncidentService : BaseService<IncidentEntity>, IIncidentsService
    {
        public IncidentService(IRepo<IncidentEntity> repo) : base(repo)
        {
        }

        public async Task<IncidentDto> CreateAsync(CreateIncidentDto dto)
        {
            var e = ToEntity(dto);

            await AddEntityAsync(e);

            return ToDto(e);
        }

        public async Task<IEnumerable<IncidentDto>> GetAllAsync()
        {
            var entities = await GetAllEntityAsync();
            return entities.Select(ToDto);
        }

        public async Task<IncidentDto?> GetByIdAsync(int id)
        {
            var e = await GetEntityByIdAsync(id);
            if (e is null) return null;
            return ToDto(e);
        }

        public async Task UpdateAsync(int id, UpdateIncidentDto dto)
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

        private static IncidentEntity ToEntity(CreateIncidentDto dto)
        {
            return new IncidentEntity
            {
                Title = dto.Title,
                Description = dto.Description,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                IsScheduled = dto.IsScheduled,
                IsSystemGenerated = dto.IsSystemGenerated,
                ServiceId = dto.ServiceId,
                AssignedEngineerId = dto.AssignedEngineerId
            };
        }

        private static IncidentDto ToDto(IncidentEntity entity)
        {
            return new IncidentDto(
                entity.Id,
                entity.Title,
                entity.Description,
                entity.StartTime,
                entity.EndTime,
                entity.IsScheduled,
                entity.IsSystemGenerated,
                entity.ServiceId,
                entity.AssignedEngineerId
            );
        }

        private static void ApplyUpdate(IncidentEntity entity, UpdateIncidentDto dto)
        {
            entity.Description = dto.Description;
            entity.StartTime = dto.StartTime;
            entity.EndTime = dto.EndTime;
            entity.IsScheduled = dto.IsScheduled;
            entity.IsSystemGenerated = dto.IsSystemGenerated;
            entity.ServiceId = dto.ServiceId;
            entity.AssignedEngineerId = dto.AssignedEngineerId;
        }

        public async Task<PagedResult<IncidentDto>> GetPagedIncidentsAsync(PaginationQuery query)
        {
            var allIncidents = await _repo.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                allIncidents = allIncidents.Where(i =>
                    i.Title.Contains(query.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            allIncidents = allIncidents.OrderByDescending(i => i.StartTime);

            int totalCount = allIncidents.Count();

            var pagedItems = allIncidents
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(i => new IncidentDto(i.Id, i.Title, i.Description, i.StartTime, i.EndTime, i.IsScheduled, i.IsSystemGenerated, i.ServiceId, i.AssignedEngineerId))
                .ToList();

            return new PagedResult<IncidentDto>
            {
                Items = pagedItems,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }
    }
}
