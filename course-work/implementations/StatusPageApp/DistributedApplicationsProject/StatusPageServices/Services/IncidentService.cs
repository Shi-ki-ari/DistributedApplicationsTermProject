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
            var e = new IncidentsEntity
            {
                Description = dto.Description,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                IsScheduled = dto.IsScheduled,
                ServiceId = dto.ServiceId,
                AssignedEngineerId = dto.AssignedEngineerId ?? 0
            };

            await AddEntityAsync(e);

            return new IncidentDto(
                e.Id,
                e.Description,
                e.StartTime,
                e.EndTime,
                e.IsScheduled,
                e.ServiceId,
                e.AssignedEngineerId == 0 ? null : (int?)e.AssignedEngineerId
            );
        }

        public async Task<IEnumerable<IncidentDto>> GetAllAsync()
        {
            var entities = await GetAllEntityAsync();
            return entities.Select(e => new IncidentDto(
                e.Id,
                e.Description,
                e.StartTime,
                e.EndTime,
                e.IsScheduled,
                e.ServiceId,
                e.AssignedEngineerId == 0 ? null : (int?)e.AssignedEngineerId
            ));
        }

        public async Task<IncidentDto?> GetByIdAsync(int id)
        {
            var e = await GetEntityById(id);
            if (e is null) return null;
            return new IncidentDto(
                e.Id,
                e.Description,
                e.StartTime,
                e.EndTime,
                e.IsScheduled,
                e.ServiceId,
                e.AssignedEngineerId == 0 ? null : (int?)e.AssignedEngineerId
            );
        }

        public async Task UpdateAsync(int id, UpdateIncidentDto dto)
        {
            var existing = await GetEntityById(id);
            if (existing is null) return;

            existing.Description = dto.Description;
            existing.StartTime = dto.StartTime;
            existing.EndTime = dto.EndTime;
            existing.IsScheduled = dto.IsScheduled;
            existing.ServiceId = dto.ServiceId;
            existing.AssignedEngineerId = dto.AssignedEngineerId ?? 0;

            await UpdateEntityAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteEntityAsync(id);
        }
    }
}
