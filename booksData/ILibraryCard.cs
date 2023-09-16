using booksData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booksData
{
    public interface ILibraryCard
    {
        // adding a new libraryCard for the new created user
        void AddLibraryCard(LibraryCard newLibraryCard);
        LibraryCard GetById(int libraryCardId);
    }
}
