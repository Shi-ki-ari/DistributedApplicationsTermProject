namespace StatusPageServices.RequestDTO.ServiceCategories
{
    public record CreateServiceCategoryDto(
        string Name,
        string? Description,
        int DisplayOrder,
        bool Notify
    );
}
