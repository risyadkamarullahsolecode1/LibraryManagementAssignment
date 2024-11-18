using LibraryManagementAssignment.Application.Interfaces;
using LibraryManagementAssignment.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAssignment.WebAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class BookManagerController : ControllerBase
    {
        private readonly IBookManagerService _bookManagerService;
        private readonly IBookManagerRepository _bookManagerRepository;

        public BookManagerController(IBookManagerService bookManagerService, IBookManagerRepository bookManagerRepository)
        {
            _bookManagerService = bookManagerService;
            _bookManagerRepository = bookManagerRepository;
        }

        [HttpPost("borrow")]
        public async Task<IActionResult> BorrowBook([FromQuery] int userId, [FromQuery] int bookId)
        {
            try
            {
                await _bookManagerService.BorrowBook(userId, bookId);
                return Ok("Book borrowed successfully.");
            }
            catch (InvalidOperationException ex)
            {
                // Handle case where the user has reached the borrowing limit
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Handle other potential exceptions
                return StatusCode(500, "An error occurred while borrowing the book.");
            }
        }

        [HttpPost("return")]
        public async Task<IActionResult> ReturnBook([FromQuery] int userId, [FromQuery] int bookId, [FromBody] DateOnly tanggalKembali)
        {
            try
            {
                await _bookManagerService.ReturnBook(userId, bookId, tanggalKembali);
                return Ok("Book returned successfully.");
            }
            catch (InvalidOperationException ex)
            {
                // Handle case where the user has reached the borrowing limit
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Handle other potential exceptions
                return StatusCode(500, "An error occurred while borrowing the book.");
            }
        }
        [HttpGet("BorrowBook")]
        public async Task<IActionResult> GetAllRecord()
        {
            var a = await _bookManagerService.GetAllBorrow();
            return Ok(a);
        }

        [HttpGet]
        public async Task<IActionResult> GetHistory(int userId)
        {
            var res = await _bookManagerRepository.GetBorrowedBooksCountByUser(userId);
            return Ok(res);
        }
    }
}
