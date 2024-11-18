using LibraryManagementAssignment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAssignment.Domain.Interfaces
{
    public interface IBookManagerRepository
    {
        Task<int> GetBorrowedBooksCountByUser(int userId);
        BookManager GetBorrowRecord(int userId, int bookId);
        Task AddBorrowRecord(BookManager borrowRecord);
        Task DeleteBorrowRecord(BookManager borrowRecord);
        Task UpdateBorrowRecord(BookManager borrow);
        Task<IEnumerable<BookManager>> GetAllBookRecord();
    }
}
