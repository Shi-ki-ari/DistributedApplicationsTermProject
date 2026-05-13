using System.Collections.Generic;
using System.Threading.Tasks;
using StatusPageServices.RequestDTO.ServiceChecks;
using StatusPageServices.ResponseDTO.ServiceChecks;

namespace StatusPageServices.Interfaces
{
    public interface IServiceChecksService
    {
        Task<IEnumerable<ServiceCheckDto>> GetAllAsync();
        Task<ServiceCheckDto?> GetByIdAsync(int id);
        Task<ServiceCheckDto> CreateAsync(CreateServiceCheckDto dto);
        Task<IEnumerable<ServiceCheckDto>> ExecuteSweepAsync();
    }
}
