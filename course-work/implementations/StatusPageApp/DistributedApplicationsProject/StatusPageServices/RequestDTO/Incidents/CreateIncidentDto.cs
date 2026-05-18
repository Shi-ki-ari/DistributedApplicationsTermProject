using System;

namespace StatusPageServices.RequestDTO.Incidents
{
    public record CreateIncidentDto(
        string Title,
        string Description,
        DateTime StartTime,
        DateTime? EndTime,
        bool IsScheduled,
        int ServiceId,
        int? AssignedEngineerId,
        bool IsSystemGenerated = false
    );
}
