using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAssignment.Application.Dto.Search
{
    public class DisplaySearchDto
    {
        public string? Category {  get; set; }
        public string? ISBN { get; set; }
        public string? Title {  get; set; }
        public string? Author { get; set; }
    }
}
