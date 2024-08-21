using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAssignment.Application.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? LibraryCardNumber { get; set; }
        public DateTime? LibraryCardExpDate { get; set; }
        public string? Position { get; set; }
        public string? Previlege { get; set; }
    }
}
