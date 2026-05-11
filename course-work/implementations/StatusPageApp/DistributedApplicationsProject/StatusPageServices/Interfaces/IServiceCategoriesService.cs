using StatusPageServices.ResponseDTO.ServiceCategories;

using StatusPageServices.RequestDTO.ServiceCategories;
using StatusPageServices.ResponseDTO.ServiceCategories;

namespace StatusPageServices.Interfaces
{
    public interface IServiceCategoriesService
    {
        Task<IEnumerable<ServiceCategoryDto>> GetAllAsync();
        Task<ServiceCategoryDto?> GetByIdAsync(int id);
        Task<ServiceCategoryDto> CreateAsync(CreateServiceCategoryDto dto);
        Task UpdateAsync(int id, UpdateServiceCategoryDto dto);
        Task DeleteAsync(int id);
    }
}
