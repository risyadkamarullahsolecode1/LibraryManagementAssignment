using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAssignment.Application.Dto.Account
{
    public class RegisterUser
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public int? LibraryCardNumber { get; set; }
        public DateOnly? LibraryCardExpDate { get; set; }
        public string? Position { get; set; }
        public string? Previlege { get; set; }
    }
}
