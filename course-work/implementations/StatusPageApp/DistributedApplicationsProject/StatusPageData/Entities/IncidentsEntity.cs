using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusPageData.Entities
{
    public class IncidentsEntity : BaseEntity
    {
        public required string Description { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool IsScheduled { get; set; }

        public int ServiceId { get; set; }

        public int AssignedEngineerId { get; set; }

        // Navigation properties

        public Services Service { get; set; } = null!;

        public EngineersEntity AssignedEngineer { get; set; } = null!;

        public ICollection<IncidentsUpdates> Updates { get; set; } = new List<IncidentsUpdates>();
    }
}
