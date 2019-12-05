using System.Collections.Generic;
using System.IO;
using System.Linq;
using BookstoreApp.API.Models;
using Newtonsoft.Json;

namespace BookstoreApp.API.Data
{
    public class Seed
    {
        public static void SeedBooks(DataContext context)
        {
            if (!context.Books.Any())
            {
                var bookData = File.ReadAllText("Data/BookSeedData.json");
                var books = JsonConvert.DeserializeObject<List<Book>>(bookData);

                foreach (var book in books)
                {
                    context.Books.Add(book);
                }

                context.SaveChanges();
            }
        }
    }
}