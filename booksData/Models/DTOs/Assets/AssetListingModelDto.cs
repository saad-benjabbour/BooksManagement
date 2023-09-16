using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booksData.Models.DTOs.Assets
{
    public class AssetListingModelDto
    {
        // What will we print out to the user
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string AuthorOrDirector { get; set; }
        public string Type { get; set; }
        public string DeweyCallNumber { get; set; }
        public int CopiesAvailable { get; set; }
    }
}
