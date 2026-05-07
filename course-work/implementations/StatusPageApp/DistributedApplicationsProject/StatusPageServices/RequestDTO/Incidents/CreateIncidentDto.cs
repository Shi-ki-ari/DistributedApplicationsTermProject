using System;

namespace StatusPageServices.RequestDTO.Incidents
{
    public record CreateIncidentDto(
        string Description,
        DateTime StartTime,
        DateTime? EndTime,
        bool IsScheduled,
        int ServiceId,
        int? AssignedEngineerId
    );
}
