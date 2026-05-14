using System;

namespace StatusPageData.Entities
{
    public class ServiceCheckEntity : BaseEntity
    {
        public int ServiceId { get; set; }

        public DateTime CheckedAt { get; set; }

        public bool IsHealthy { get; set; }

        public int? ResponseTimeMs { get; set; }

        public int? StatusCode { get; set; }

        public string? ErrorMessage { get; set; }

        // Navigation
        public ServiceEntity Service { get; set; } = null!;
    }
}
