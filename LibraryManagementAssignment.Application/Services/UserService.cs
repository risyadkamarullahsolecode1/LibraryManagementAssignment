using LibraryManagementAssignment.Application.Dto.Account;
using LibraryManagementAssignment.Application.Interfaces;
using LibraryManagementAssignment.Domain.Entities;
using LibraryManagementAssignment.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAssignment.Application.Services
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(IUserRepository userRepository,UserManager<AppUser> userManager ,RoleManager<IdentityRole> roleManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task AttachNotes(int id, string notes)
        {
            var note = await _userRepository.GetUserById(id);
            if (note == null)
            {
                throw new Exception($"user with Id {id} not found");
            }
            note.Note = notes;

            _userRepository.UpdateUser(note);
            await _userRepository.SaveChangesAsync();
        }

        // Add user + user for sign up(registration)
        public async Task<ResponseModel> ResigtrationUser(RegisterUser registerUser)
        {
            var user = await _userRepository.GetUserById(registerUser.Id);
            if (user != null) return new ResponseModel { Status = "Error", Message = "User already exists!" };

            AppUser userApp = new AppUser()
            {
                Email = registerUser.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerUser.FirstName+""+registerUser.LastName
            };

            var result = await _userManager.CreateAsync(userApp, registerUser.Password);
            if (!result.Succeeded) return new ResponseModel
            {
                Status = "Error",
                Message = "User creation failed! Please check user details and try again."
            };
            var userId = userApp.Id;

            User users = new User()
            {
                Id = registerUser.Id,
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                LibraryCardNumber = registerUser.LibraryCardNumber,
                LibraryCardExpDate = registerUser.LibraryCardExpDate,
                Position = registerUser.Position,
                Previlege = registerUser.Previlege,
                AppUserId = userId,
            };
            await _userRepository.AddUser(users);
            await _userRepository.SaveChangesAsync();
            return new ResponseModel { Status = "Success", Message = "User created succesfully!" };
        }
    }
}
