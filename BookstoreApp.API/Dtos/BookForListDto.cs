using System;

namespace BookstoreApp.API.Dtos
{
    public class BookForListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateModified { get; set; }
        public string PhotoUrl { get; set; }
    }
}