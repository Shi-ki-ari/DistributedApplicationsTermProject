using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusPageData.Entities
{
    public class IncidentsUpdates : BaseEntity
    {
        public required string Message { get; set; }

        public DateTime PostedAt { get ; set; }

        public required string UpdateStatus { get;set; }

        public bool IsSystemGenerated { get; set; }

        public int IncidentId { get; set; }

        public int EngineerId { get; set; }
        // Navigation properties
        public IncidentsEntity Incident { get; set; } = null!;

        public EngineersEntity Engineer { get; set; } = null!;

    }
}
