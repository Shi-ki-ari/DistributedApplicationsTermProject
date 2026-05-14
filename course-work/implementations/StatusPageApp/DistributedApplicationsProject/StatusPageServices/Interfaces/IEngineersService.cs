using StatusPageServices.ResponseDTO.Engineers;
using StatusPageServices.RequestDTO.Engineers;


namespace StatusPageServices.Interfaces
{
    public interface IEngineersService
    {
        Task<IEnumerable<EngineerDto>> GetAllAsync();
        Task<EngineerDto?> GetByIdAsync(int id);
        Task<EngineerDto> CreateAsync(CreateEngineerDto dto);
        Task UpdateAsync(int id, UpdateEngineerDto dto);
        Task DeleteAsync(int id);
    }
}
