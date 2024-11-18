using LibraryManagementAssignment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAssignment.Application.Interfaces
{
    public interface IBookManagerService
    {
        Task BorrowBook(int userId, int bookId);
        Task ReturnBook(int userId, int bookId, DateOnly tanggalKembali);
        Task<IEnumerable<BookManager>> GetAllBorrow();
    }
}
