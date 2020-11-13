using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryManagement.Objects
{
    public class Book
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public bool HasBeenBorrowed { get; set; }
    }
}