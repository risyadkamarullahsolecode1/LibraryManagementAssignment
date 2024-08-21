using LibraryManagementAssignment.Application.Dto;
using LibraryManagementAssignment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementAssignment.Application.Mappers
{
    public static class BookMapper
    {
        public static BookDto ToBookDto(this Book book)
        {
            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.ISBN,
                Author = book.Author,
                Category = book.Category,
                Publisher = book.Publisher,
                Description = book.Description,
                Language = book.Language,
                Location = book.Location,
                PurchaseDate = book.PurchaseDate,
                Price = book.Price,
                TotalBook = book.TotalBook
            };
        }
    }
}
