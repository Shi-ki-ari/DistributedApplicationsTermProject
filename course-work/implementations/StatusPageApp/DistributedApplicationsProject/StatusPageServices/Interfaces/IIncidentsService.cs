using StatusPageServices.ResponseDTO.Incidents;

using StatusPageServices.RequestDTO.Incidents;
using StatusPageServices.ResponseDTO.Incidents;

namespace StatusPageServices.Interfaces
{
    public interface IIncidentsService
    {
        Task<IEnumerable<IncidentDto>> GetAllAsync();
        Task<IncidentDto?> GetByIdAsync(int id);
        Task<IncidentDto> CreateAsync(CreateIncidentDto dto);
        Task UpdateAsync(int id, UpdateIncidentDto dto);
        Task DeleteAsync(int id);
    }
}
