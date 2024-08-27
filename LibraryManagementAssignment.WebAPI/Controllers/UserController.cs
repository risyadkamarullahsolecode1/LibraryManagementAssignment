using LibraryManagementAssignment.Application.Interfaces;
using LibraryManagementAssignment.Application.Mappers;
using LibraryManagementAssignment.Domain.Entities;
using LibraryManagementAssignment.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAssignment.WebAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        public UserController(IUserRepository userRepository, IUserService userService)
        {
            _userRepository = userRepository;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllUser()
        {
            var user = await _userRepository.GetAllUser();
            var userDto = user.Select(u => u.ToUserDto()).ToList();
            return Ok(userDto);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> AddUser(User user)
        {
            var createdUser = await _userRepository.AddUser(user);
            return CreatedAtAction(nameof(AddUser), createdUser);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> UpdateUser(int id, User user)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var updatedUser = await _userRepository.UpdateUser(user);
            var userDto = updatedUser.ToUserDto();
            if (userDto == null)
            {
                return BadRequest();
            }
            return Ok(userDto);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteUser(int id)
        {
            var deleted = await _userRepository.DeleteUser(id);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok("User telah dihapus");
        }
        [HttpPut("note/{id}")]
        public async Task<IActionResult> AttachNotes(int id, string notes)
        {
            await _userService.AttachNotes(id, notes);
            return Ok();
        }
    }
}
