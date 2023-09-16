using booksData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booksData
{
    public interface IPatron
    {
        Patron Get(int patronId);
        IEnumerable<Patron> GetAll();
        void AddPatron(Patron newPatron);
        IEnumerable<CheckoutHistory> getCheckoutHistory(int patronId);
        IEnumerable<Hold> getHolds(int patronId);
        IEnumerable<Checkout> getCheckouts(int patronId);
    }
}
