using booksData;
using booksData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksManagementService
{
    public class PatronService : IPatron
    {
        private readonly booksDBContext _context;

        public PatronService(booksDBContext context)
        {
            _context = context;
        }

        public void AddPatron(Patron newPatron)
        {
            _context.Add(newPatron);
            _context.SaveChanges();
        }

        public IEnumerable<Patron> GetAll()
        {
            return _context.Patrons
                .Include(a => a.libraryCard)
                .Include(a => a.libraryBranch);
        }
        public Patron Get(int patronId)
        {
            return GetAll()
                .FirstOrDefault(a => a.Id == patronId.ToString());
        }


        public IEnumerable<CheckoutHistory> getCheckoutHistory(int patronId)
        {
            var cardId = _context.Patrons
                .Include(a => a.libraryCard)
                .FirstOrDefault(a => a.Id == patronId.ToString())
                .libraryCard.Id;
            return _context.checkOutHistories
                .Include(a => a.LibraryCard)
                .Include(a => a.LibraryAsset)
                .Where(condition => condition.LibraryCard.Id == cardId)
                .OrderByDescending(a => a.checkedOut);
        }

        public IEnumerable<Checkout> getCheckouts(int patronId)
        {
            var cardId = _context.Patrons
                .Include(a => a.libraryCard)
                .FirstOrDefault(a => a.Id == patronId.ToString())
                .libraryCard.Id;
            return _context.Checkouts
                .Include(a => a.libraryCard)
                .Include(a => a.libraryAsset)
                .Where(condition => condition.libraryCard.Id == cardId)
                .OrderByDescending(a => a.since);
        }

        public IEnumerable<Hold> getHolds(int patronId)
        {
            var cardId = _context.Patrons
                .Include(a => a.libraryCard)
                .FirstOrDefault(condition => condition.Id == patronId.ToString())
                .libraryCard.Id;

            return _context.holds
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .Where(condition => condition.LibraryCard.Id == cardId)
                .OrderByDescending(a => a.HoldPlaced);
        }
    }
}
