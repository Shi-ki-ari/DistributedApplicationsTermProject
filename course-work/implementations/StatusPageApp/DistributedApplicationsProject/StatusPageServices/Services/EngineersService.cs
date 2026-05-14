using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StatusPageData.Entities;
using StatusPageRepo;
using StatusPageServices.Interfaces;
using StatusPageServices.RequestDTO.Engineers;
using StatusPageServices.ResponseDTO.Engineers;

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


    }
}
