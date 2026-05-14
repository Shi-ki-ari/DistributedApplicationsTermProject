using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusPageData.Entities
{
    public class ServiceCategoryEntity : BaseEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }

        public int DisplayOrder { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool Notify { get; set; }
        // Navigation properties
        public ICollection<ServiceEntity> Services { get; set; } = new List<ServiceEntity>();
    }
}
