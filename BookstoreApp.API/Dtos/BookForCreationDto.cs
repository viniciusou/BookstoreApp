using System;

namespace BookstoreApp.API.Dtos
{
    public class BookForCreationDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateModified { get; set; }

        public BookForCreationDto()
        {
            DateAdded = DateTime.Now;
            DateModified = DateAdded;
        }
    }
}