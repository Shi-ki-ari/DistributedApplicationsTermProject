using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusPageData.Entities
{
    public class EngineerEntity : BaseEntity
    {

        public required string Name { get; set; }

        public required string Email { get; set; }

        public DateTime HiredDate { get; set; }

        public bool OnCall { get; set; }

        public double HourlyRate { get; set; }

        // Navigation properties
        public ICollection<IncidentEntity> AssignedIncidents { get; set; } = new List<IncidentEntity>();

        public ICollection<IncidentUpdateEntity> IncidentUpdates { get; set; } = new List<IncidentUpdateEntity>();

    }
}
