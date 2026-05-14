using System;

namespace StatusPageServices.RequestDTO.IncidentUpdates
{
    public record UpdateIncidentUpdateDto(
        int Id,
        string Message,
        string UpdateStatus,
        bool IsSystemGenerated,
        int IncidentId,
        int EngineerId
    );
}
