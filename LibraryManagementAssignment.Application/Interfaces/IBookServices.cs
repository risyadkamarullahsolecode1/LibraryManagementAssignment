using LibraryManagementAssignment.Application.Dto.Search;
using LibraryManagementAssignment.Domain.Entities;
using LibraryManagementAssignment.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAssignment.Application.Interfaces
{
    public interface IBookServices
    {
        Task<Book> AddBookAsync(Book book);
        Task<IEnumerable<Book>> SearchBookLanguage(string language);
        Task DeleteStampBook(int id, string deleteStatus);
        Task<IEnumerable<DisplaySearchDto>> SearchBooksAsync(SearchDto query, Pagination pagination);
        Task<DetailBookDto> GetBookDetailsAsync(int bookId);
    }
}
