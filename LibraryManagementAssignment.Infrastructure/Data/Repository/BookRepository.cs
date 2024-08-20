using LibraryManagementAssignment.Domain.Entities;
using LibraryManagementAssignment.Domain.Helpers;
using LibraryManagementAssignment.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAssignment.Infrastructure.Data.Repository
{
    public class BookRepository:IBookRepository
    {
        private readonly LibraryContext _context;

        public BookRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllBooks(QueryObject query)
        {
            var books = _context.Books.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Title))
            {
                books = books.Where(b => b.Title.Contains(query.Title));
            }
            if (!string.IsNullOrWhiteSpace(query.ISBN))
            {
                books = books.Where(b => b.ISBN.Contains(query.ISBN));
            }
            if (!string.IsNullOrWhiteSpace(query.Author))
            {
                books = books.Where(b => b.Author.Contains(query.Author));
            }
            if (!string.IsNullOrWhiteSpace(query.Category))
            {
                books = books.Where(b => b.Category.Contains(query.Category));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Title", StringComparison.OrdinalIgnoreCase))
                {
                    books = query.IsDescending ? books.OrderByDescending(T => T.Title) : books.OrderBy(T => T.Title);
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await _context.Books.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Book> GetBookById(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<Book> AddBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<Book> UpdateBook(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<bool> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return false;
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
