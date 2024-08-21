using LibraryManagementAssignment.Application.Dto;
using LibraryManagementAssignment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAssignment.Application.Mappers
{
    public static class UserMapper
    {
        public static UserDto ToUserDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                LibraryCardNumber = user.LibraryCardNumber,
                LibraryCardExpDate = user.LibraryCardExpDate,
                Position = user.Position,
                Previlege = user.Previlege
            };
        }
    }
}
