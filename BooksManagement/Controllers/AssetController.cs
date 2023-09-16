using booksData;
using booksData.Models.DTOs.Assets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BooksManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        private ILibraryAsset _asset;
        private ICheckout _checkout;

        public AssetController(ILibraryAsset asset, ICheckout checkout)
        {
            _asset = asset;
            _checkout = checkout;
        }

        // Getting all the assets available in the Database
        [HttpGet]
        public IActionResult GetAssets()
        {
            var assets = _asset.GetAll();
            var listingResults = assets
                .Select(result => new AssetListingModelDto
                {
                    // mapping the results of GetAll to the properties of AssetListingModelDto
                    Id = result.Id,
                    ImageUrl = result.ImageUrl,
                    AuthorOrDirector = _asset.GetAuthorOrDirector(result.Id),
                    DeweyCallNumber = _asset.GetDeweyIndex(result.Id),
                    Title = _asset.GetTitle(result.Id),
                    Type = _asset.GetType(result.Id),
                    CopiesAvailable = result.NumberOfCopies
                });
            if (listingResults == null)
                return BadRequest("Something went wrong in GetAssets() function");
            return Ok(listingResults);
        }

        // Getting a specific asset by Id, printing out the details of an asset
        
        // api/GetByAssetId/id
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetAssetById(int id)
        {
            var asset = _asset.getById(id);
            var currentHolds = _checkout.GetCurrentHolds(id)
                 .Select(a => new AssetHoldModel
                 {
                     HoldPlaced = _checkout.GetCurrentHoldPlace(a.Id).ToString("d"),
                     PatronName = _checkout.GetCurrentHoldPatronName(a.Id)
                 });

            var assetDetail = new AssetDetailModelDto
            {
                AssetId = asset.Id,
                Title = asset.Title,
                Cost = asset.Cost,
                Status = asset.Status.Name,
                ImageUrl = asset.ImageUrl,
                AuthorOrDirector = _asset.GetAuthorOrDirector(id),
                CurrentLocation = _asset.GetCurrentLocation(id).Name,
                DeweyCallNumber = _asset.GetDeweyIndex(id),
                Type = _asset.GetType(id),
                ISBN = _asset.GetIsbn(id),
                checkoutHistory = _checkout.GetCheckoutHistory(id),
                LatestCheckout = _checkout.GetLatestCheckout(id), 
                PatronName = _checkout.GetCurrentCheckoutPatron(id),
                currentHolds = currentHolds
            };
            return Ok(assetDetail);
        }

        // printing out the holds being placed on a asset

        // api/Hold/id
        [HttpGet]
        [Route("Hold/{id}")]
        public IActionResult Hold(int id)
        {
            var asset = _asset.getById(id);
            // var hold = _checkout.GetById(id);
            var model = new CheckoutModelDto
            {
                AssetId = id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = 0,
                IsCheckedOut = _checkout.isCheckedOut(id),
                HoldCount = _checkout.GetCurrentHolds(id).Count()
            };
            return Ok(model);
        }
        
        [HttpGet]
        [Route("Checkout/{id}")]
        public IActionResult Checkout(int id)
        {
            var asset = _asset.getById(id);
            // var checkout = _checkout.GetById(id);
            // var libraryCardId = _asset.;
            var model = new CheckoutModelDto
            {
                AssetId = asset.Id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = 0,
                IsCheckedOut = _checkout.isCheckedOut(id)
            };
            return Ok(model);
        }
        
        [HttpPost]
        [Route("MarkFound")]
        public IActionResult MarkFound(int assetId)
        {
            _checkout.MarkFound(assetId);
            return NoContent();
        }
        
        [HttpPost]
        [Route("MarkLost")]
        public IActionResult MarkLost(int assetId)
        {
            _checkout.MarkLost(assetId);
            return NoContent();
        }

        // placing a hold on an asset using the assetId which we gonna take it from the link and libraryCardId
        // which we gonna take from the user input
        [HttpPost]
        [Route("PlaceHold")]
        // api/Asset/PlaceHold?assetId=2&libraryCardId=9
        public IActionResult PlaceHold(int assetId, int libraryCardId)
        {
            _checkout.PlaceHold(assetId, libraryCardId);
            return NoContent();
        }

        [HttpPost]
        [Route("PlaceCheckout")]
        public IActionResult PlaceCheckout(int assetId, int LibraryCardId)
        {
            _checkout.CheckOutItem(assetId, LibraryCardId);
            return NoContent();
        }

        [HttpPost]
        [Route("CheckIn")]
        public IActionResult CheckIn(int assetId)
        {
            _checkout.CheckInItem(assetId);
            return NoContent();
        }
        
    }

}
