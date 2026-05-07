using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusPageData.Entities
{
    public class EngineersEntity : BaseEntity
    {

        public required string Name { get; set; }

        public required string Email { get; set; }

        public DateTime HiredDate { get; set; }

        public bool OnCall { get; set; }

        public double HourlyRate { get; set; }

        // Navigation properties
        public ICollection<IncidentsEntity> AssignedIncidents { get; set; } = new List<IncidentsEntity>();

        public ICollection<IncidentsUpdates> IncidentUpdates { get; set; } = new List<IncidentsUpdates>();

    }
}
