using booksData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booksData
{
    public interface ILibraryAsset
    {
        IEnumerable<LibraryAsset> GetAll();

        LibraryAsset getById(int assetId);

        void AddAsset(LibraryAsset newAsset);

        string GetAuthorOrDirector(int assetId);

        string GetDeweyIndex(int assetId);
        string GetType(int assetId);
        string GetTitle(int assetId);
        string GetIsbn(int assetId);
        string GetAuthor(int assetId);
        string GetDirector(int assetId);

        LibraryBranch GetCurrentLocation(int assetId);
        LibraryCard GetLibraryCardByAssetId(int assetId);
    }
}
