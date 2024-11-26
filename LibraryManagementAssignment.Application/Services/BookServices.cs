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

        public async Task<IEnumerable<Book>> GetAllBookAsync(Pagination pagination)
        {
            var books = await _bookRepository.GetAllBooks();
            // If pagination is null or neither pageSize nor pageNumber is provided, return all jobs
            if (pagination == null || (pagination.PageSize == null && pagination.PageNumber == null))
            {
                return books; // Return all job posts without pagination
            }

            // Apply default values for pagination if only one of pageSize or pageNumber is provided
            var pageSize = pagination.PageSize ?? 10;  // Default page size to 10 if not provided
            var pageNumber = pagination.PageNumber ?? 1;  // Default page number to 1 if not provided

            // Ensure pageSize and pageNumber are valid
            pageSize = Math.Max(1, pageSize);  // Ensure page size is at least 1
            pageNumber = Math.Max(1, pageNumber);  // Ensure page number is at least 1

            var skipNumber = (pageNumber - 1) * pageSize;

            // Return the paginated result
            return books.Skip(skipNumber).Take(pageSize);
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
                var total = books.Count();
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

        public async Task<object> SearchBooksBasicAsync(QueryObject query)
        {
            var temp = _bookRepository.GetBooksQueryable();

            if (!string.IsNullOrEmpty(query.Keyword))
            {
                temp = temp.Where(b =>
                b.Title.ToLower().Contains(query.Keyword.ToLower()) ||
                b.Author.ToLower().Contains(query.Keyword.ToLower()) ||
                b.ISBN.ToLower().Contains(query.Keyword.ToLower()) ||
                b.Subject.ToLower().Contains(query.Keyword.ToLower()) ||
                b.Category.ToLower().Contains(query.Keyword.ToLower())
                );
            }
            if (!string.IsNullOrEmpty(query.Title))
                temp = temp.Where(b => b.Title.ToLower().Contains(query.Title.ToLower()));
            if (!string.IsNullOrEmpty(query.Author))
                temp = temp.Where(b => b.Author.ToLower().Contains(query.Author.ToLower()));
            if (!string.IsNullOrEmpty(query.ISBN))
                temp = temp.Where(b => b.ISBN.ToLower().Contains(query.ISBN.ToLower()));
            if (!string.IsNullOrEmpty(query.Subject))
                temp = temp.Where(b => b.Subject.ToLower().Contains(query.Subject.ToLower()));
            if (!string.IsNullOrEmpty(query.Category))
                temp = temp.Where(b => b.Category.ToLower().Contains(query.Category.ToLower()));
            var total = temp.Count();

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                switch (query.SortBy)
                {
                    case "author":
                        temp = query.SortOrder.Equals("asc") ? temp.OrderBy(s => s.Author) :
                        temp.OrderByDescending(s => s.Author);
                        break;
                    case "isbn":
                        temp = query.SortOrder.Equals("asc") ? temp.OrderBy(s => s.ISBN) :
                        temp.OrderByDescending(s => s.ISBN);
                        break;
                    case "id":
                        temp = query.SortOrder.Equals("asc") ? temp.OrderBy(s => s.Id) :
                        temp.OrderByDescending(s => s.Id);
                        break;
                    default:
                        temp = query.SortOrder.Equals("asc") ? temp.OrderBy(s => s.Title) :
                        temp.OrderByDescending(s => s.Title);
                        break;
                }
            }
            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            var list = await temp.Skip(skipNumber).Take(query.PageSize).ToListAsync();
            return new { total = total, data = list };
        }
    }
}
