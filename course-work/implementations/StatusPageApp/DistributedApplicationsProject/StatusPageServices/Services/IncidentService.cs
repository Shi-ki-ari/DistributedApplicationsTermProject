using StatusPageData.Entities;
using StatusPageData.Entities;
using StatusPageRepo;
using StatusPageServices.Interfaces;
using StatusPageServices.RequestDTO.Incidents;
using StatusPageServices.ResponseDTO.Incidents;
namespace StatusPageServices.Services
{
    public class IncidentService : BaseService<IncidentsEntity>, IIncidentsService
    {
        public IncidentService(IRepo<IncidentsEntity> repo) : base(repo)
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
            var e = await GetEntityById(id);
            if (e is null) return null;
            return ToDto(e);
        }

        public async Task UpdateAsync(int id, UpdateIncidentDto dto)
        {
            var existing = await GetEntityById(id);
            if (existing is null) return;

            ApplyUpdate(existing, dto);

            await UpdateEntityAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteEntityAsync(id);
        }

        private static IncidentsEntity ToEntity(CreateIncidentDto dto)
        {
            return new IncidentsEntity
            {
                Description = dto.Description,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                IsScheduled = dto.IsScheduled,
                ServiceId = dto.ServiceId,
                AssignedEngineerId = dto.AssignedEngineerId ?? 0
            };
        }

        private static IncidentDto ToDto(IncidentsEntity entity)
        {
            return new IncidentDto(
                entity.Id,
                entity.Description,
                entity.StartTime,
                entity.EndTime,
                entity.IsScheduled,
                entity.ServiceId,
                entity.AssignedEngineerId == 0 ? null : (int?)entity.AssignedEngineerId
            );
        }

        private static void ApplyUpdate(IncidentsEntity entity, UpdateIncidentDto dto)
        {
            entity.Description = dto.Description;
            entity.StartTime = dto.StartTime;
            entity.EndTime = dto.EndTime;
            entity.IsScheduled = dto.IsScheduled;
            entity.ServiceId = dto.ServiceId;
            entity.AssignedEngineerId = dto.AssignedEngineerId ?? 0;
        }
    }
}
