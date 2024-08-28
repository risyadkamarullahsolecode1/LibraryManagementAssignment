using LibraryManagementAssignment.Application.Dto.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAssignment.Application.Interfaces
{
    public interface IUserService
    {
        Task AttachNotes(int id, string notes);
        Task<ResponseModel> ResigtrationUser(RegisterUser registerUser);
    }
}
