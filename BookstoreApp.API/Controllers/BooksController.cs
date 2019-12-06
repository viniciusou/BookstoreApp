using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookstoreApp.API.Data;
using BookstoreApp.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookstoreRepository _repo;
        private readonly IMapper _mapper;
        public BooksController(IBookstoreRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _repo.GetBooks();

            var booksToReturn = _mapper.Map<IEnumerable<BookForListDto>>(books);
            
            return Ok(booksToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await _repo.GetBook(id);

            var bookToReturn = _mapper.Map<BookForDetailedDto>(book);
            
            return Ok(bookToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, BookForUpdateDto bookForUpdateDto)
        {
            var bookFromRepo = await _repo.GetBook(id);

            _mapper.Map(bookForUpdateDto, bookFromRepo);

            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception($"Updating book {id} failed on save");
        }
    }
}