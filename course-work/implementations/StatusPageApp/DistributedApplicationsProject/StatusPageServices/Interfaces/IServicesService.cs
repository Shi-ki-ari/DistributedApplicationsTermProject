using StatusPageServices.RequestDTO.Services;
using StatusPageServices.ResponseDTO.Services;

namespace StatusPageServices.Interfaces
{
    public interface IServicesService
    {
        Task<IEnumerable<ServiceDto>> GetAllAsync();
        Task<ServiceDto?> GetByIdAsync(int id);
        Task<ServiceDto> CreateAsync(CreateServiceDto dto);
        Task UpdateAsync(int id, UpdateServiceDto dto);
        Task DeleteAsync(int id);
    }
}
