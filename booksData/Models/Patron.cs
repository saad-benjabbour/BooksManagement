using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace booksData.Models
{
  
    public class Patron : IdentityUser
    {
        // adding the columns of our user (Patron)
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? Address { get; set; }
        public DateTime? dateOfBirth { get; set; }
        // public string? phoneNumber { get; set; }
        [NotMapped]
        public string Discriminator { get; set; }

        // foreign key
        public virtual LibraryCard libraryCard { get; set; } // a patron has one libraryCard
        public virtual LibraryBranch libraryBranch { get; set; } // a patron belongs to one libraryBranch
    }
}
