using System;
using System.ComponentModel.DataAnnotations;

namespace booksData.Models
{
    public class CheckoutHistory
    {
        public int id { get; set; }

        [Required]
        public LibraryAsset LibraryAsset { get; set; }
        [Required]
        public LibraryCard LibraryCard { get; set; }

        [Required]
        public DateTime checkedOut { get; set; }
        public DateTime? checkedIn { get; set; }
    }
}
