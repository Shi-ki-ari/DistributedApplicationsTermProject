namespace StatusPageServices.RequestDTO.Services
{
    public record UpdateServiceDto(
        int Id,
        string Name,
        string TargetUrl,
        int CategoryId,
        bool IsOnline
    );
}
