using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAssignment.Application.Dto.Search
{
    public class SearchDto
    {
        public string? Title { get; set; }
        public string? Category { get; set; }
        public string? ISBN { get; set; }
        public string? Language { get; set; }
        public string? Subject { get; set; }
        public string? QueryOperators { get; set; }
        public string? SortBy { get; set; } = null;
        public string? SortOrder { get; set; } = "asc";
    }
}
