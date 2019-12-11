using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookstoreApp.API.Data;
using BookstoreApp.API.Dtos;
using BookstoreApp.API.Helpers;
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

        [HttpGet(Name="GetBooks")]
        public async Task<IActionResult> GetBooks([FromQuery]BookParams bookParams)
        {
            var books = await _repo.GetBooks(bookParams);

            var booksToReturn = _mapper.Map<IEnumerable<BookForListDto>>(books);
            
            if (books != null)
            {
                Response.AddPagination(books.CurrentPage, books.PageSize, 
                    books.TotalCount, books.TotalPages);
            }

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