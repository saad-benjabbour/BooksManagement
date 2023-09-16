using booksData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booksData
{
    public interface ICheckout
    {
        // CRUD operations for checkout service
        IEnumerable<Checkout> GetAll();
        Checkout GetById(int checkoutId);
        void Add(Checkout newCheckout);

        void CheckOutItem(int assetId, int libraryCard);

        void CheckInItem(int assetId);

        IEnumerable<CheckoutHistory> GetCheckoutHistory(int id);
        void PlaceHold(int assetId, int libraryCard);
        string GetCurrentHoldPatronName(int holdId);
        DateTime GetCurrentHoldPlace(int holdId);
        IEnumerable<Hold> GetCurrentHolds(int assetId);
        void MarkLost(int assetId);
        void MarkFound(int assetId);
        Checkout GetLatestCheckout(int assetId);
        string GetCurrentCheckoutPatron(int assetId);
        bool isCheckedOut(int assetId);
    }
}
