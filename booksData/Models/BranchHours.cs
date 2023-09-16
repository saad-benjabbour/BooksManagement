using System;
using System.ComponentModel.DataAnnotations;

namespace booksData.Models
{
    public class BranchHours
    {
        // Working Time for each branch
        public int id { get; set; }
        public LibraryBranch Branch { get; set; }
        [Range(0, 6)]
        public int dayOfWeek { get; set; }
        [Range(0, 23)]
        public int openTime { get; set; }
        [Range(0, 23)]
        public int closeTime { get; set; }
    }
}
