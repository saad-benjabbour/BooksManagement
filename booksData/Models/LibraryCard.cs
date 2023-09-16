using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace booksData.Models
{
    public class LibraryCard
    {
        public int Id { get; set; }
        [Display(Name ="Overdue Fees")] public decimal Fees { get; set; }
        [Display(Name = "Card Issued Date")] public DateTime Created { get; set; }

        // foreign key
        [Display(Name = "Materials on Loan")] public virtual IEnumerable<Checkout> Checkouts { get; set; }

        public static implicit operator LibraryCard(int v)
        {
            throw new NotImplementedException();
        }
    }
}
