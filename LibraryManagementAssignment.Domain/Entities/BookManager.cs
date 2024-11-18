using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAssignment.Domain.Entities
{
    public class BookManager
    {
        [Key]
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateOnly TanggalPinjam { get; set; }
        public DateOnly TanggalKembali { get; set; }
        public DateOnly? DueDate { get; set; }
        public int? Penalty { get; set; }
    }
}
