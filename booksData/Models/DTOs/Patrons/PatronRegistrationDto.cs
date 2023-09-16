using System;
using System.ComponentModel.DataAnnotations;

namespace booksData.Models.DTOs
{
    public class PatronRegistrationDto
    {
        // the data that a patron needs in order for him to register
        [Required]
        public string  firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string phoneNumber { get; set; }
        [Required]
        public DateTime dateOfBirth { get; set; }
    }
}
