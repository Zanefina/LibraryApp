﻿using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models.DTO
{
    public class BookDTO
    {
        public int BookId { get; set; }
        
        public string Title { get; set; }
        
        public string Author { get; set; }
        
        public DateTime? PublishedOn { get; set; }
        
        public string Genre { get; set; }
        
        public string Description { get; set; }
      
        public bool Availability { get; set; }
    }
}
