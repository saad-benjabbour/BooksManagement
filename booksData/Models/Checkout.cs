using System;
using System.ComponentModel.DataAnnotations;

namespace booksData.Models
{
    public class Checkout
    {
        public int id { get; set; }

        // foreign key
        [Required]
        public LibraryAsset libraryAsset { get; set; }
        public LibraryCard libraryCard { get; set; }

        public DateTime since { get; set; }
        public DateTime until { get; set; }
    }
}
