﻿using LibraryManagementAssignment.Application.Dto.Account;
using LibraryManagementAssignment.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAssignment.WebAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.SignUpAsync(model);

            if(result.Status == "Error")
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)

                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(model);
            if (result.Status == "Error")

                return BadRequest(result.Message);

            return Ok(result);
        }
        [HttpPost("set-role")]
        public async Task<IActionResult> CreateRoleAsync(string rolename)
        {
            var result = await _authService.CreateRoleAsync(rolename);
            return Ok(result);
        }
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignToRoleAsync(string userName, [FromBody]string rolename)
        {
            var result = await _authService.AssignToRoleAsync(userName, rolename);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateRoleAsync(string userName, [FromBody] string rolename)
        {
            var result = await _authService.UpdateToRoleAsync(userName, rolename);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteRoleAsync(string userName, [FromBody] string rolename)
        {
            var result = await _authService.DeleteToRoleAsync(userName, rolename);
            return Ok(result);
        }
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync(string userName)
        {
            var result = await _authService.LogoutAsync(userName);
            return Ok(result);
        }
    }
}
