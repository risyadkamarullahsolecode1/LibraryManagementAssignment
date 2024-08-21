using LibraryManagementAssignment.Domain.Entities;
using LibraryManagementAssignment.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAssignment.Application.Interfaces
{
    public interface IBookServices
    {
       Task<IEnumerable<Book>> SearchBookLanguage(string language);
       Task<bool> DeleteStampBook(int id, string deleteStatus);
    }
}
