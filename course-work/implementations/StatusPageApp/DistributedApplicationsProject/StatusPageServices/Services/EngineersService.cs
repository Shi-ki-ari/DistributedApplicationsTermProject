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
    public class EngineersService : BaseService<EngineersEntity>, IEngineersService
    {
        public EngineersService(IRepo<EngineersEntity> repo) : base(repo)
        {
        }

        public async Task<EngineerDto> CreateAsync(CreateEngineerDto dto)
        {
            var e = new EngineersEntity
            {
                Name = dto.Name,
                Email = dto.Email,
                HiredDate = dto.HiredDate ?? System.DateTime.UtcNow,
                OnCall = dto.OnCall,
                HourlyRate = dto.HourlyRate
            };

            await AddEntityAsync(e);

            return new EngineerDto(e.Id, e.Name, e.Email, e.HiredDate, e.OnCall, e.HourlyRate);
        }

        public async Task<IEnumerable<EngineerDto>> GetAllAsync()
        {
            var entities = await GetAllEntityAsync();
            return entities.Select(e => new EngineerDto(
                e.Id,
                e.Name,
                e.Email,
                e.HiredDate,
                e.OnCall,
                e.HourlyRate
            ));
        }

        public async Task<EngineerDto?> GetByIdAsync(int id)
        {
            var e = await GetEntityById(id);
            if (e is null) return null;
            return new EngineerDto(e.Id, e.Name, e.Email, e.HiredDate, e.OnCall, e.HourlyRate);
        }

        public async Task UpdateAsync(int id, UpdateEngineerDto dto)
        {
            var existing = await GetEntityById(id);
            if (existing is null) return;

            existing.Name = dto.Name;
            existing.Email = dto.Email;
            existing.HiredDate = dto.HiredDate;
            existing.OnCall = dto.OnCall;
            existing.HourlyRate = dto.HourlyRate;

            await UpdateEntityAsync(existing);
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteEntityAsync(id);
        }


    }
}
