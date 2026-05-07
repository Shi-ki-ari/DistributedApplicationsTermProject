using System;

namespace StatusPageServices.ResponseDTO.IncidentUpdates
{
    public record IncidentUpdateDto(
        int Id,
        string Message,
        DateTime PostedAt,
        string UpdateStatus,
        bool IsSystemGenerated,
        int IncidentId,
        int EngineerId
    );
}
