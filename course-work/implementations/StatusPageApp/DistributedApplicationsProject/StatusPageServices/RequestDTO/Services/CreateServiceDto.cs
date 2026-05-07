namespace StatusPageServices.RequestDTO.Services
{
    public record CreateServiceDto(
        string Name,
        string TargetUrl,
        int CategoryId,
        bool IsOnline
    );
}
