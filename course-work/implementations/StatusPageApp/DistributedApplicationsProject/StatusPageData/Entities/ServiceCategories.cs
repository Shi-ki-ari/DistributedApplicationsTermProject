using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusPageData.Entities
{
    public class ServiceCategories
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; }

        public int DisplayOrder { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool Notify { get; set; }
        // Navigation properties
        public ICollection<Services> Services { get; set; } = new List<Services>();
    }
}
