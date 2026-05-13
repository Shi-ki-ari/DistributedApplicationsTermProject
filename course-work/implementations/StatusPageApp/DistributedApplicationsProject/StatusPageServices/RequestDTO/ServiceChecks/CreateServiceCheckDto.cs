using System;

namespace StatusPageServices.RequestDTO.ServiceChecks
{
    public record CreateServiceCheckDto(
        int ServiceId,
        DateTime CheckedAt,
        bool IsHealthy,
        int? ResponseTimeMs,
        int? StatusCode,
        string? ErrorMessage
    );
}
