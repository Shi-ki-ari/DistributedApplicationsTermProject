namespace StatusPageRazorUI.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    }

    public record EngineerDto(
        int Id,
        string Name,
        string Email,
        DateTime HiredDate,
        bool OnCall,
        double HourlyRate);

    public record ServiceDto(
        int Id,
        string Name,
        string TargetUrl,
        bool IsOnline,
        DateTime DateAdded,
        decimal UptimePercentage,
        int CategoryId);

    public record ServiceCategoryDto(
        int Id,
        string Name,
        string? Description,
        int DisplayOrder,
        DateTime CreatedAt,
        bool Notify);

    public record IncidentUpdateDto(
        int Id,
        string Message,
        DateTime PostedAt,
        string UpdateStatus,
        int IncidentId,
        int EngineerId);

    public record UserDto(
        int Id,
        string Username);

    public record IncidentDto(
        int Id,
        string Title,
        string Description,
        DateTime StartTime,
        DateTime? EndTime,
        bool IsScheduled,
        bool IsSystemGenerated,
        int ServiceId,
        int? AssignedEngineerId);

    public record ServiceCheckDto(
        int Id,
        int ServiceId,
        DateTime CheckedAt,
        bool IsHealthy,
        int? ResponseTimeMs,
        int? StatusCode,
        string? ErrorMessage);
}
