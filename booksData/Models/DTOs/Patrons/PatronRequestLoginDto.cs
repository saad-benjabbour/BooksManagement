using System.ComponentModel.DataAnnotations;

namespace booksData.Models.DTOs
{
    public class PatronRequestLoginDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
