using System;

namespace StatusPageServices.ResponseDTO.Services
{
    public record ServiceDto(
        int Id,
        string Name,
        string TargetUrl,
        bool IsOnline,
        DateTime DateAdded,
        decimal UptimePercentage,
        int CategoryId
    );
}
