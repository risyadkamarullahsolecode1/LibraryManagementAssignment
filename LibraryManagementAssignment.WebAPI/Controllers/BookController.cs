﻿using LibraryManagementAssignment.Application.Interfaces;
using LibraryManagementAssignment.Domain.Entities;
using LibraryManagementAssignment.Domain.Helpers;
using LibraryManagementAssignment.Domain.Interfaces;
using LibraryManagementAssignment.Application.Mappers;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementAssignment.Application.Dto;
using Microsoft.AspNetCore.Authorization;
using LibraryManagementAssignment.Application.Dto.Search;
using LibraryManagementAssignment.Application.Services;

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
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks([FromQuery]Pagination pagination)
        {
            var books = await _bookServices.GetAllBookAsync(pagination);
            var bookDto = books.Select(b => b.ToBookDto()).ToList();
            return Ok(bookDto);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBookById(int id)
        {
            var book = await _bookRepository.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }
            var bookDto = book.ToBookDto();
            return Ok(bookDto);
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
            var bookDto = createdBook.ToBookDto();
            return Ok(bookDto);
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
        
        [HttpGet("search-book-language")]
        public async Task <ActionResult<IEnumerable<Book>>> SearchBookLanguage([FromQuery]string language)
        {
            var booklanguage = await _bookServices.SearchBookLanguage(language);
            var booklanguageDto = booklanguage.Select(x => x.ToBookDto()).ToList();
            return Ok(booklanguageDto);
        }
        [HttpPut("delete-stamp/{id}")]
        public async Task<ActionResult> DeleteStampBook(int id, string deleteStatus)
        {
            await _bookServices.DeleteStampBook(id, deleteStatus);
            return Ok(new { id, deleteStatus });
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchBooks([FromQuery] SearchDto searchDto, [FromQuery] Pagination pagination)
        {
            try
            {
                var results = await _bookServices.SearchBooksAsync(searchDto, pagination);
                return Ok(results);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetBookDetails(int id)
        {
            try
            {
                var bookDetails = await _bookServices.GetBookDetailsAsync(id);
                return Ok(bookDetails);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("search-basic")]
        public async Task<IActionResult> SearchBookBasic([FromQuery]QueryObject query)
        {
            try
            {
                var bookDetails = await _bookServices.SearchBooksBasicAsync(query);
                return Ok(bookDetails);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
