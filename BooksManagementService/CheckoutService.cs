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
    public class CheckoutService : ICheckout
    {
        private readonly booksDBContext _context;

        public CheckoutService(booksDBContext context)
        {
            _context = context;
        }


        public void Add(Checkout newCheckout)
        {
            _context.Add(newCheckout);
            _context.SaveChanges();
        }

        public IEnumerable<Checkout> GetAll()
        {
            return _context.Checkouts;
        }

        public Checkout GetById(int checkoutId)
        {
            return GetAll().FirstOrDefault(condition => condition.id == checkoutId);
        }
        public Hold GetHoldById(int holdId)
        {
            return _context.holds
                .FirstOrDefault(condition => condition.Id == holdId);
        }

        public IEnumerable<CheckoutHistory> GetCheckoutHistory(int id)
        {
            return _context.checkOutHistories
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .Where(condition => condition.LibraryAsset.Id == id);
        }

        /* Helpfull functions for the checkInItem */
        private void RemoveCheckouts(int assetId)
        {
            var _asset = _context.Checkouts
                .FirstOrDefault(condition => condition.libraryAsset.Id == assetId);
            if (_asset != null)
                _context.Remove(_asset);
        }

        private void CloseCheckoutHistory(int assetId, DateTime now)
        {
            var _assetHitory = _context.checkOutHistories
                .FirstOrDefault(condition => condition.LibraryAsset.Id == assetId
                                && condition.checkedIn == null);
            if(_assetHitory != null)
            {
                _context.Update(_assetHitory);
                _assetHitory.checkedIn = now; // the patron has return the asset just about now
            }
        }
        public void CheckInItem(int assetId)
        {
            DateTime now = DateTime.Now;
            // 1 - Retrieving the asset in question
            var asset = _context.libraryAssets
                .FirstOrDefault(condition => condition.Id == assetId);
            // mark the item for an update
            _context.Update(asset);
            // 2- Remove the checkout
            RemoveCheckouts(assetId);
            // 3- Closing the checkoutHistory
            CloseCheckoutHistory(assetId, now);
            // 4- checking if there is any holds on that asset
            var currentHodls = _context.holds
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .Where(condition => condition.LibraryAsset.Id == assetId);  
            if(currentHodls.Any())
            {
                CheckingOutToEarliestHold(assetId, currentHodls);
            }

            // update the status of the asset to "available"
            updateStatus(assetId, "Available");
            _context.SaveChanges();

        }

        private void CheckingOutToEarliestHold(int assetId, IQueryable<Hold> currentHodls)
        {
            // getting the earliest hold
            var earliestHold = currentHodls
                .OrderBy(condition => condition.HoldPlaced)
                .FirstOrDefault();
            var libraryCardId = earliestHold.LibraryCard.Id;
            _context.Remove(earliestHold);
            _context.SaveChanges();
            // checkingOut the asset
            CheckOutItem(assetId, libraryCardId);
        }

        /* helpfull functions for CheckoutItem */
        public bool isCheckedOut(int assetId)
        {
            return _context.Checkouts
                .Where(a => a.libraryAsset.Id == assetId)
                .Any();
        }
        private DateTime GetDefaultCheckoutItem(DateTime now)
        {
            return now.AddDays(30);
        }
        public void CheckOutItem(int assetId, int libraryCardId)
        {
            DateTime now = DateTime.Now;
            // we can't checkout an item that's already checkedOut
            if (isCheckedOut(assetId))
            {
                return;
            }

            // getting the asset
            var item = _context.libraryAssets
                .FirstOrDefault(a => a.Id == assetId);
            updateStatus(assetId, "Checked Out");

            // getting the libraryCard
            var libraryCard = _context.libraryCards
                .Include(card => card.Checkouts)
                .FirstOrDefault(condition => condition.Id == libraryCardId);


            // adding a checkout of this asset in the database
            var checkout = new Checkout
            {
                libraryAsset = item,
                libraryCard = libraryCard,
                since = now,
                until = GetDefaultCheckoutItem(now)
            };
            _context.Add(checkout);
            var checkoutHistory = new CheckoutHistory
            {
                checkedOut = now,
                LibraryAsset = item,
                LibraryCard = libraryCard
            };
            _context.Add(checkoutHistory);
            _context.SaveChanges();
        }
        public string GetCurrentCheckoutPatron(int assetId)
        {
            var checkout = GetCheckoutByAssetId(assetId);


            if (checkout == null)
                return "not checkedOut";
            var cardId = checkout.libraryCard.Id;

            var patron = _context.Patrons
                .Include(p => p.libraryCard)
                .FirstOrDefault(condition => condition.libraryCard.Id == cardId);

            return patron.firstName + " " + patron.lastName;
        }

        private Checkout GetCheckoutByAssetId(int assetId)
        {
            return _context.Checkouts
               .Include(a => a.libraryAsset)
               .Include(a => a.libraryCard)
               .FirstOrDefault(condition => condition.libraryAsset.Id == assetId);
        }

        public string GetCurrentHoldPatronName(int holdId)
        {
            var hold = _context.holds
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .FirstOrDefault(condition => condition.Id == holdId);

            // getting the libraryCardId
            var cardId = hold.LibraryCard.Id;

            var patron = _context.Patrons
                .Include(c => c.libraryCard)
                .FirstOrDefault(condition => condition.libraryCard.Id == cardId);

            return patron.firstName + "" + patron.lastName;
        }

        public DateTime GetCurrentHoldPlace(int holdId)
        {
            return _context.holds
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .FirstOrDefault(condition => condition.Id == holdId)
                .HoldPlaced;
        }

        public IEnumerable<Hold> GetCurrentHolds(int assetId)
        {
            return _context.holds
                .Include(a => a.LibraryAsset)
                .Where(condition => condition.LibraryAsset.Id == assetId);
        }

        public Checkout GetLatestCheckout(int assetId)
        {
            return _context.Checkouts
               .Where(condition => condition.libraryAsset.Id == assetId)
               .OrderByDescending(c => c.since)
               .FirstOrDefault();
        }

        public void MarkFound(int assetId)
        {
            DateTime now = DateTime.Now;
            updateStatus(assetId, "Available");
            RemoveCheckouts(assetId);
            CloseCheckoutHistory(assetId, now);
            _context.SaveChanges();
        }

        private void updateStatus(int assetId, string statusName)
        {
            var asset = _context.libraryAssets
                .FirstOrDefault(condition => condition.Id == assetId);

            _context.Update(asset);

            asset.Status = _context.Status
                .FirstOrDefault(condition => condition.Name == "statusName");
            _context.SaveChanges();
        }

        public void MarkLost(int assetId)
        {
            updateStatus(assetId, "Available");
            _context.SaveChanges();

        }

        public void PlaceHold(int assetId, int libraryCardId)
        {
            DateTime now = DateTime.Now;
            // 1- retrieving the libraryCard in question
            var _libraryCard = _context.libraryCards
                .FirstOrDefault(condition => condition.Id == libraryCardId);

            // 2- retrieving the asset in question and its status in include
            var _asset = _context.libraryAssets
                .Include(a => a.Status)
                .FirstOrDefault(condition => condition.Id == assetId);

            if(_asset.Status.Name == "Available")
            {
                updateStatus(_asset.Id, "On Hold");
            }

            var hold = new Hold
            {
                LibraryCard = _libraryCard,
                LibraryAsset = _asset,
                HoldPlaced = now
            };
            _context.Add(hold);
            _context.SaveChanges();
        }
    }
}
