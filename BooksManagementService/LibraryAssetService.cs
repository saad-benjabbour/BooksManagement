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
    public class LibraryAssetService : ILibraryAsset
    {
        private readonly booksDBContext _context;

        public LibraryAssetService(booksDBContext context)
        {
            _context = context;
        }

        public void AddAsset(LibraryAsset newAsset)
        {
            _context.Add(newAsset);
            _context.SaveChanges();
        }

        // we've called this method in AssetController
        // and it seems to be working just fine
        public IEnumerable<LibraryAsset> GetAll()
        {
            // Getting libraryAssets, its status and which libraryBranch its in
            return _context.libraryAssets
                .Include(a => a.Status)
                .Include(a => a.Location)
                .OrderBy(a => a.Id);
        }

        public string GetAuthor(int assetId)
        {
            var book = (Book)getById(assetId);
            return book.Author;
        }

        public string GetAuthorOrDirector(int assetId)
        {
            var isBook = GetType(assetId);
            if (isBook == "Book")
                return GetAuthor(assetId);
            else if (isBook == "Video")
                return GetDirector(assetId);
            else
                return "Uknown Type";

        }
        // we've called this method in AssetController
        // and it seems to be working just fine
        public LibraryAsset getById(int assetId)
        {
            // condition represent the whole select query result
            // its the concept of lambda expression
            return _context.libraryAssets
                .Include(a => a.Status)
                .Include(a => a.Location)
                .FirstOrDefault(condition => condition.Id == assetId);
        }

        public LibraryBranch GetCurrentLocation(int assetId)
        {
            return _context.libraryAssets
                .First(a => a.Id == assetId).Location;
        }

        public string GetDeweyIndex(int assetId)
        {
            if (GetType(assetId) == "Book")
            {
                var book = (Book)getById(assetId);
                return book.DeweyIndex;
            }
            else
                return "Not Available";
        }

        public string GetDirector(int assetId)
        {
            var video = (Video)getById(assetId);
            return video.Director;
        }

        public string GetIsbn(int assetId)
        {
            if (GetType(assetId) == "Book")
            {
                var book = (Book)getById(assetId);
                return book.ISBN.ToString();
            }
            else
                return "Not Available";
        }

        public LibraryCard GetLibraryCardByAssetId(int assetId)
        {
            // we'd like to know if the libraryAsset has any checkouts by a specific libraryCard
            return _context.libraryCards
                .FirstOrDefault(c => c.Checkouts
                .Select(a => a.libraryAsset)
                .Select(condition => condition.Id).Contains(assetId));
        }

        public string GetTitle(int assetId)
        {
            return _context.libraryAssets
                .FirstOrDefault(a => a.Id == assetId)
                .Title;
        }

        public string GetType(int assetId)
        {
            var isBook = _context.libraryAssets
                .OfType<Book>().SingleOrDefault(a => a.Id == assetId);

            return isBook != null ? "Book" : "Video" ?? "Unkown Type";
        }
    }
}
