using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace booksData.Models
{
    public class LibraryBranch
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Branch Name")]
        [StringLength(30, ErrorMessage = "Limit branch name to 30 characters.")]
        public string Name { get; set; }

        [Required] public string Address { get; set; }

        [Required] public string Telephone { get; set; }

        public string Description { get; set; }
        public DateTime OpenDate { get; set; }

        public string imageUrl { get; set; }

        // foreign key
        public virtual IEnumerable<Patron> Patrons { get; set; } // libraryBranch has many patrons subscribed to it
        public virtual IEnumerable<LibraryAsset> libraryAssets { get; set; } // libraryBranch has many  libraryAssets

    }
}
