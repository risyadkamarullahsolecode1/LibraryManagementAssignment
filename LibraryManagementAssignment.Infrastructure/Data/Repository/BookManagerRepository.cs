using LibraryManagementAssignment.Domain.Entities;
using LibraryManagementAssignment.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAssignment.Infrastructure.Data.Repository
{
    public class BookManagerRepository:IBookManagerRepository
    {
        private readonly LibraryContext _context;

        public BookManagerRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookManager>> GetAllBookRecord()
        {
            var book = await _context.BookManagers.ToListAsync();
            return book;
        }

        public async Task<int> GetBorrowedBooksCountByUser(int userId)
        {
            return await _context.BookManagers
                         .Where(lb => lb.UserId == userId && lb.TanggalKembali >= DateOnly.FromDateTime(DateTime.Now))
                         .CountAsync();
        }

        public BookManager GetBorrowRecord(int userId, int bookId)
        {
            return _context.BookManagers.FirstOrDefault(b => b.UserId == userId && b.BookId == bookId);
        }

        public async Task AddBorrowRecord(BookManager borrowRecord)
        {
            _context.BookManagers.AddAsync(borrowRecord);
            await _context.SaveChangesAsync();
            return;
        }

        public async Task DeleteBorrowRecord(BookManager borrowRecord)
        {
            _context.BookManagers.Remove(borrowRecord);
            _context.SaveChanges();
        }
        public async Task UpdateBorrowRecord(BookManager borrow)
        {
            _context.BookManagers.Update(borrow);
            await _context.SaveChangesAsync();
        }
    }
}
