using System;

namespace StatusPageServices.ResponseDTO.ServiceCategories
{
    public record ServiceCategoryDto(
        int Id,
        string Name,
        string? Description,
        int DisplayOrder,
        DateTime CreatedAt,
        bool Notify
    );
}
