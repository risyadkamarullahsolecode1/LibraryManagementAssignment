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

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
    }
}
