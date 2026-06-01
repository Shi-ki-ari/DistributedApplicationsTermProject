using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusPageServices.RequestDTO
{
    public class PaginationQuery
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? SearchTerm { get; set; }

        public string? SearchEmail { get; set; }
        public string? SearchTargetUrl { get; set; }

        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }
    }
}
