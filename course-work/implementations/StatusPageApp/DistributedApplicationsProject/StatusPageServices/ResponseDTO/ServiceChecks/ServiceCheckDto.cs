using System;

namespace StatusPageServices.ResponseDTO.ServiceChecks
{
    public record ServiceCheckDto(
        int Id,
        int ServiceId,
        DateTime CheckedAt,
        bool IsHealthy,
        int? ResponseTimeMs,
        int? StatusCode,
        string? ErrorMessage
    );
}
