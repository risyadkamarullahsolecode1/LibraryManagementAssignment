using LibraryManagementAssignment.Application.Dto.Search;
using LibraryManagementAssignment.Application.Interfaces;
using LibraryManagementAssignment.Domain.Entities;
using LibraryManagementAssignment.Domain.Helpers;
using LibraryManagementAssignment.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Book> AddBookAsync(Book book)
        {
            await _bookRepository.AddBook(book);
            return book;
        }

       public async Task<IEnumerable<Book>> SearchBookLanguage(string language)
        {
            var languagebook = await _bookRepository.GetAllBooks();

            return languagebook
                .Where(book => book.Language == language)
                .ToList();
        }

        public async Task DeleteStampBook(int id, string deleteStatus)
        {
            var deleted = await _bookRepository.GetBookById(id);
            if (deleted == null)
            {
                throw new Exception($"Book with Id {id} not found");
            }
            deleted.DeleteStamp = true;
            deleted.DeleteStatus = deleteStatus;

            _bookRepository.UpdateBook(deleted);
            await _bookRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<DisplaySearchDto>> SearchBooksAsync(SearchDto query, Pagination pagination)
        {
            try
            {
                // Get all books as a queryable
                var books = _bookRepository.GetBooksQueryable();

                // Apply filters if query parameters are provided
                if (query != null)
                {
                    // Apply OR-based filtering logic if specified
                    if (!string.IsNullOrEmpty(query.QueryOperators) && query.QueryOperators.Equals("OR", StringComparison.OrdinalIgnoreCase))
                    {
                        books = books.Where(b =>
                            (string.IsNullOrEmpty(query.Title) || b.Title.ToLower().Contains(query.Title.ToLower())) ||
                            (string.IsNullOrEmpty(query.ISBN) || b.ISBN.ToLower().Contains(query.ISBN.ToLower())) ||
                            (string.IsNullOrEmpty(query.Category) || b.Category.ToLower().Contains(query.Category.ToLower())) ||
                            (string.IsNullOrEmpty(query.Language) || b.Language.ToLower().Contains(query.Language.ToLower())) ||
                            (string.IsNullOrEmpty(query.Subject) || b.Subject.ToLower().Contains(query.Subject.ToLower()))
                        );
                    }
                    else
                    {
                        // Apply AND-based filtering logic (default behavior)
                        if (!string.IsNullOrEmpty(query.Title))
                            books = books.Where(b => b.Title.ToLower().Contains(query.Title.ToLower()));

                        if (!string.IsNullOrEmpty(query.ISBN))
                            books = books.Where(b => b.ISBN.ToLower().Contains(query.ISBN.ToLower()));

                        if (!string.IsNullOrEmpty(query.Category))
                            books = books.Where(b => b.Category.ToLower().Contains(query.Category.ToLower()));

                        if (!string.IsNullOrEmpty(query.Language))
                            books = books.Where(b => b.Language.ToLower().Contains(query.Language.ToLower()));

                        if (!string.IsNullOrEmpty(query.Subject))
                            books = books.Where(b => b.Subject.ToLower().Contains(query.Subject.ToLower()));
                    }
                }

                // Sort books by Title (default sort order)
                books = books.OrderBy(b => b.Title);

                // Apply pagination
                if (pagination != null)
                {
                    var pageSize = pagination.PageSize ?? 10; // Default to 10 if PageSize is not provided
                    var pageNumber = pagination.PageNumber ?? 1; // Default to page 1 if PageNumber is not provided

                    // Ensure valid values for PageSize and PageNumber
                    pageSize = Math.Max(1, pageSize);
                    pageNumber = Math.Max(1, pageNumber);

                    var skipNumber = (pageNumber - 1) * pageSize;

                    // Apply pagination
                    books = books.Skip(skipNumber).Take(pageSize);
                }

                // Fetch results as a list
                var result = await books.ToListAsync();

                // Map results to DisplaySearchDto
                return result.Select(b => new DisplaySearchDto
                {
                    Title = b.Title,
                    ISBN = b.ISBN,
                    Author = b.Author,
                    Category = b.Category
                }).ToList();
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                throw new ApplicationException("An error occurred while searching, filtering, or sorting books.", ex);
            }
        }


        public async Task<DetailBookDto> GetBookDetailsAsync(int bookId)
        {
            try
            {
                var book = await _bookRepository.GetBookByIdAsync(bookId);
                if (book == null)
                    throw new KeyNotFoundException("Book not found.");

                // Map the book to DetailBookDto
                return new DetailBookDto
                {
                    Title = book.Title,
                    ISBN = book.ISBN,
                    Author = book.Author,
                    Category = book.Category,
                    Publisher = book.Publisher,
                    Description = book.Description,
                    Language = book.Language
                };
            }
            catch (Exception ex)
            {
                // Log the error here as needed
                throw new ApplicationException("An error occurred while retrieving book details.", ex);
            }
        }
    }
}
