using System;

namespace StatusPageServices.ResponseDTO.Incidents
{
    public record IncidentDto(
        int Id,
        string Description,
        DateTime StartTime,
        DateTime? EndTime,
        bool IsScheduled,
        int ServiceId,
        int? AssignedEngineerId
    );
}
