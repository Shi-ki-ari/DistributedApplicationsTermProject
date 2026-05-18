using System;

namespace StatusPageServices.RequestDTO.IncidentUpdates
{
    public record UpdateIncidentUpdateDto(
        int Id,
        string Message,
        string UpdateStatus,
        int IncidentId,
        int EngineerId
    );
}
