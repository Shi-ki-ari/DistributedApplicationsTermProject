using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StatusPageData.Entities;
using StatusPageRepo;
using StatusPageServices.Interfaces;
using StatusPageServices.RequestDTO.IncidentUpdates;
using StatusPageServices.ResponseDTO.IncidentUpdates;

namespace StatusPageServices.Services
{
    public class IncidentUpdatesService : BaseService<IncidentsUpdates>, IIncidentUpdatesService
    {
        public IncidentUpdatesService(IRepo<IncidentsUpdates> repo) : base(repo)
        {
        }

        public async Task<IncidentUpdateDto> CreateAsync(CreateIncidentUpdateDto dto)
        {
            var e = new IncidentsUpdates
            {
                Message = dto.Message,
                PostedAt = System.DateTime.UtcNow,
                UpdateStatus = dto.UpdateStatus,
                IsSystemGenerated = dto.IsSystemGenerated,
                IncidentId = dto.IncidentId,
                EngineerId = dto.EngineerId
            };

            await AddEntityAsync(e);

            return new IncidentUpdateDto(
                e.Id,
                e.Message,
                e.PostedAt,
                e.UpdateStatus,
                e.IsSystemGenerated,
                e.IncidentId,
                e.EngineerId
            );
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteEntityAsync(id);
        }

        public async Task<IEnumerable<IncidentUpdateDto>> GetAllAsync()
        {
            var entities = await GetAllEntityAsync();
            return entities.Select(e => new IncidentUpdateDto(
                e.Id,
                e.Message,
                e.PostedAt,
                e.UpdateStatus,
                e.IsSystemGenerated,
                e.IncidentId,
                e.EngineerId
            ));
        }

        public async Task<IncidentUpdateDto?> GetByIdAsync(int id)
        {
            var e = await GetEntityById(id);
            if (e is null) return null;
            return new IncidentUpdateDto(e.Id, e.Message, e.PostedAt, e.UpdateStatus, e.IsSystemGenerated, e.IncidentId, e.EngineerId);
        }

        public async Task UpdateAsync(int id, UpdateIncidentUpdateDto dto)
        {
            var existing = await GetEntityById(id);
            if (existing is null) return;

            existing.Message = dto.Message;
            existing.UpdateStatus = dto.UpdateStatus;
            existing.IsSystemGenerated = dto.IsSystemGenerated;
            existing.IncidentId = dto.IncidentId;
            existing.EngineerId = dto.EngineerId;

            await UpdateEntityAsync(existing);
        }

    }
}
