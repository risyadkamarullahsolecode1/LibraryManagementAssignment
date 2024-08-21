using LibraryManagementAssignment.Domain.Entities;
using LibraryManagementAssignment.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementAssignment.Domain.Helpers;

namespace LibraryManagementAssignment.Domain.Interfaces
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllBooks();
        Task<Book> GetBookById(int id);
        Task<Book> AddBook(Book book);
        Task<Book> UpdateBook(Book book);
        Task<bool> DeleteBook(int id);
        Task<IEnumerable<Book>> SearchBookAsync(QueryObject query, Pagination pagination);
        Task<Book> SearchBookLanguage(string language, Pagination p);
    }
}
