using StatusPageServices.RequestDTO;
using StatusPageServices.RequestDTO.Engineers;
using StatusPageServices.ResponseDTO;
using StatusPageServices.ResponseDTO.Engineers;


namespace StatusPageServices.Interfaces
{
    public interface IEngineersService
    {
        Task<IEnumerable<EngineerDto>> GetAllAsync();
        Task<EngineerDto?> GetByIdAsync(int id);
        Task<EngineerDto> CreateAsync(CreateEngineerDto dto);
        Task UpdateAsync(int id, UpdateEngineerDto dto);
        Task DeleteAsync(int id);

        Task<PagedResult<EngineerDto>> GetPagedEngineersAsync(PaginationQuery query);
    }
}
