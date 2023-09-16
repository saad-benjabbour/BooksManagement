using booksData;
using booksData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksManagementService
{
    public class LibraryCardService : ILibraryCard
    {
        private readonly booksDBContext _context;

        public LibraryCardService(booksDBContext context)
        {
            _context = context;
        }

        public void AddLibraryCard(LibraryCard newLibraryCard)
        {
            _context.Add(newLibraryCard);
            _context.SaveChanges();
        }

        public LibraryCard GetById(int libraryCardId)
        {
            return _context.libraryCards
                .FirstOrDefault(condition => condition.Id == libraryCardId);
        }
    }
}
