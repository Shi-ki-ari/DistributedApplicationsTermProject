namespace StatusPageServices.RequestDTO.IncidentUpdates
{
    public record CreateIncidentUpdateDto(
        string Message,
        string UpdateStatus,
        bool IsSystemGenerated,
        int IncidentId,
        int EngineerId
    );
}
