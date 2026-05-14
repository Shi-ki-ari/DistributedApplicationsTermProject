using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusPageData.Entities
{
    public class ServiceEntity : BaseEntity
    {
        public required string Name { get; set; }

        public required string TargetUrl { get; set; }
        public bool IsOnline { get; set; }

        public DateTime DateAdded { get; set; }

        public decimal UptimePercentage { get; set; }

        public int CategoryId { get; set; }
        // Navigation properties
        public ServiceCategoryEntity Category { get; set; } = null!;

        public ICollection<IncidentEntity> Incidents { get; set; } = new List<IncidentEntity>();

    }
}
