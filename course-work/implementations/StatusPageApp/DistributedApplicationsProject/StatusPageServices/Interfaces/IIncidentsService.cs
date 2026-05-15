using StatusPageServices.RequestDTO;
using StatusPageServices.RequestDTO.Incidents;
using StatusPageServices.ResponseDTO;
using StatusPageServices.ResponseDTO.Engineers;
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

        Task<PagedResult<IncidentDto>> GetPagedIncidentsAsync(PaginationQuery query);
    }
}
