using LibraryManagementAssignment.Application.Interfaces;
using LibraryManagementAssignment.Domain.Entities;
using LibraryManagementAssignment.Domain.Helpers;
using LibraryManagementAssignment.Domain.Interfaces;
using LibraryManagementAssignment.Application.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAssignment.WebAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBookServices _bookServices;

        public BookController(IBookRepository bookRepository, IBookServices bookServices)
        {
            _bookRepository = bookRepository;
            _bookServices = bookServices;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            var books = await _bookRepository.GetAllBooks();
            return Ok(books);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBookById(int id)
        {
            var book = await _bookRepository.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }
        [HttpPost]
        public async Task<ActionResult<Book>> AddBook(Book book)
        {
            var createdBook = await _bookRepository.AddBook(book);
            return Ok(createdBook);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, Book book)
        {
            if (id != book.Id) return BadRequest();

            var createdBook = await _bookRepository.UpdateBook(book);
            return Ok(createdBook);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var deleted = await _bookRepository.DeleteBook(id);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok("Buku telah dihapus");
        }
        [HttpGet("search-book")]
        public async Task<ActionResult<IEnumerable<Book>>> SearchBookAsync([FromQuery]QueryObject query,[FromQuery] Pagination pagination)
        {
            var querybook = await _bookRepository.SearchBookAsync(query, pagination);
            return Ok(querybook);
        }
        [HttpGet("search-book-language")]
        public async Task <ActionResult<IEnumerable<Book>>> SearchBookLanguage([FromQuery]string language)
        {
            var booklanguage = await _bookServices.SearchBookLanguage(language);
            return Ok(booklanguage);
        }
        [HttpGet("language-book")]
        public async Task<ActionResult<IEnumerable<Book>>> SearchBookLanguageAsync([FromQuery]string language, [FromQuery]Pagination p)
        {
            var languagebook = await _bookRepository.SearchBookLanguage(language, p);
            return Ok(languagebook);
        }
    }
}
