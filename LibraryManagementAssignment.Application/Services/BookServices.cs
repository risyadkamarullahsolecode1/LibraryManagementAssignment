using LibraryManagementAssignment.Application.Interfaces;
using LibraryManagementAssignment.Domain.Entities;
using LibraryManagementAssignment.Domain.Helpers;
using LibraryManagementAssignment.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAssignment.Application.Services
{
    public class BookServices:IBookServices
    {
        private readonly IBookRepository _bookRepository;

        public BookServices(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

       public async Task<IEnumerable<Book>> SearchBookLanguage(string language)
        {
            var languagebook = await _bookRepository.GetAllBooks();

            return languagebook
                .Where(book => book.Language == language)
                .ToList();
        }

        public async Task<bool> DeleteStampBook(int id, string deleteStatus)
        {
            var deleted = await _bookRepository.GetBookById(id);
            if (deleted == null)
            {
                return false;
            }
            return deleted.DeleteStatus == deleteStatus;
        }
    }
}
