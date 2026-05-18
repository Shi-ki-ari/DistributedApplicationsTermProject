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
    public class IncidentUpdatesService : BaseService<IncidentUpdateEntity>, IIncidentUpdatesService
    {
        public IncidentUpdatesService(IRepo<IncidentUpdateEntity> repo) : base(repo)
        {
        }

        public async Task<IncidentUpdateDto> CreateAsync(CreateIncidentUpdateDto dto)
        {
            var e = ToEntity(dto);

            await AddEntityAsync(e);

            return ToDto(e);
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteEntityAsync(id);
        }

        public async Task<IEnumerable<IncidentUpdateDto>> GetAllAsync()
        {
            var entities = await GetAllEntityAsync();
            return entities.Select(ToDto);
        }

        public async Task<IncidentUpdateDto?> GetByIdAsync(int id)
        {
            var e = await GetEntityByIdAsync(id);
            if (e is null) return null;
            return ToDto(e);
        }

        public async Task UpdateAsync(int id, UpdateIncidentUpdateDto dto)
        {
            var existing = await GetEntityByIdAsync(id);
            if (existing is null) return;

            ApplyUpdate(existing, dto);

            await UpdateEntityAsync(existing);
        }

        private static IncidentUpdateEntity ToEntity(CreateIncidentUpdateDto dto)
        {
            return new IncidentUpdateEntity
            {
                Message = dto.Message,
                PostedAt = System.DateTime.UtcNow,
                UpdateStatus = dto.UpdateStatus,
                IncidentId = dto.IncidentId,
                EngineerId = dto.EngineerId
            };
        }

        private static IncidentUpdateDto ToDto(IncidentUpdateEntity entity)
        {
            return new IncidentUpdateDto(
                entity.Id,
                entity.Message,
                entity.PostedAt,
                entity.UpdateStatus,
                entity.IncidentId,
                entity.EngineerId
            );
        }

        private static void ApplyUpdate(IncidentUpdateEntity entity, UpdateIncidentUpdateDto dto)
        {
            entity.Message = dto.Message;
            entity.UpdateStatus = dto.UpdateStatus;
            entity.IncidentId = dto.IncidentId;
            entity.EngineerId = dto.EngineerId;
        }

    }
}
