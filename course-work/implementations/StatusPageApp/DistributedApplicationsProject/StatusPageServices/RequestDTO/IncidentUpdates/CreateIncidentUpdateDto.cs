namespace StatusPageServices.RequestDTO.IncidentUpdates
{
    public record CreateIncidentUpdateDto(
        string Message,
        string UpdateStatus,
        int IncidentId,
        int EngineerId
    );
}
