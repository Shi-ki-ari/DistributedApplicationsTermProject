using StatusPageServices.ResponseDTO.IncidentUpdates;

using StatusPageServices.RequestDTO.IncidentUpdates;
using StatusPageServices.ResponseDTO.IncidentUpdates;

namespace StatusPageServices.Interfaces
{
    public interface IIncidentUpdatesService
    {
        Task<IEnumerable<IncidentUpdateDto>> GetAllAsync();
        Task<IncidentUpdateDto?> GetByIdAsync(int id);
        Task<IncidentUpdateDto> CreateAsync(CreateIncidentUpdateDto dto);
        Task UpdateAsync(int id, UpdateIncidentUpdateDto dto);
        Task DeleteAsync(int id);
    }
}
