using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusPageData.Entities
{
    public class IncidentEntity : BaseEntity
    {
        public required string Description { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool IsScheduled { get; set; }

        public int ServiceId { get; set; }

        public int AssignedEngineerId { get; set; }

        // Navigation properties

        public ServiceEntity Service { get; set; } = null!;

        public EngineerEntity AssignedEngineer { get; set; } = null!;

        public ICollection<IncidentUpdateEntity> Updates { get; set; } = new List<IncidentUpdateEntity>();
    }
}
