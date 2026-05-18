using System;

namespace StatusPageServices.ResponseDTO.IncidentUpdates
{
    public record IncidentUpdateDto(
        int Id,
        string Message,
        DateTime PostedAt,
        string UpdateStatus,
        int IncidentId,
        int EngineerId
    );
}
