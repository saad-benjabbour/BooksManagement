using System;
using System.ComponentModel.DataAnnotations;

namespace booksData.Models
{
    public class Video : LibraryAsset
    {
        [Required] public string Director { get; set; }
    }
}
