using System;
using System.ComponentModel.DataAnnotations;

namespace booksData.Models
{
    public class Book : LibraryAsset
    {
        [Required]
        public int ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string DeweyIndex { get; set; }
    }
}
