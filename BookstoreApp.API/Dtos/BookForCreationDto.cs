using System;
using System.ComponentModel.DataAnnotations;

namespace BookstoreApp.API.Dtos
{
    public class BookForCreationDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string Publisher { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime DateReleased { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateModified { get; set; }

        public BookForCreationDto()
        {
            DateAdded = DateTime.Now;
            DateModified = DateAdded;
        }
    }
}