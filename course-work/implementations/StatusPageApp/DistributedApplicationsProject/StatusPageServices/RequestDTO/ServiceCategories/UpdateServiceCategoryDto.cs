namespace StatusPageServices.RequestDTO.ServiceCategories
{
    public record UpdateServiceCategoryDto(
        int Id,
        string Name,
        string? Description,
        int DisplayOrder,
        bool Notify
    );
}
