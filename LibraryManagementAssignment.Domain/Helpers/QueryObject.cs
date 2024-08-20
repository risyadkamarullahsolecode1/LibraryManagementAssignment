using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAssignment.Domain.Helpers
{
    public class QueryObject
    {
        public string? ISBN { get; set; }
        public string? Category { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? SortBy {  get; set; }
        public bool IsDescending { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; }
    }
}
