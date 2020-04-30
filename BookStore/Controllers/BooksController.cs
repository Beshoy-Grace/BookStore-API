using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.Contracts;
using BookStore.Data;
using BookStore.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        public BooksController(IBookRepository bookRepository,
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooks()
        {
          //  var location = GetControllerActionNames();
            try
            {
        
                var books = await _bookRepository.FindAll();
                var response = _mapper.Map<IList<BookDTO>>(books);

                return Ok(response);
            }
            catch (Exception e)
            {
                //  return InternalError($"{location}: {e.Message} - {e.InnerException}");
                return BadRequest("Error........");
            }
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBook(int id)
        {
            var location = GetControllerActionNames();
            try
            {
               
                var book = await _bookRepository.FindById(id);
                if (book == null)
                {
                   
                    return NotFound();
                }
                var response = _mapper.Map<BookDTO>(book);

                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }
        /// <summary>
        /// Creates a new book
        /// </summary>
        /// <param name="bookDTO"></param>
        /// <returns>Book Object</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] BookCreateDTO bookDTO)
        {
            var location = GetControllerActionNames();
            try
            {
           
                if (bookDTO == null)
                {
                 
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                  
                    return BadRequest(ModelState);
                }
                var book = _mapper.Map<Book>(bookDTO);
                var isSuccess = await _bookRepository.Create(book);
                if (!isSuccess)
                {
                    return InternalError($"{location}: Creation failed");
                }
              
                return Created("Create", new { book });
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] BookUpdateDTO bookDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                
                if (id < 1 || bookDTO == null || id != bookDTO.Id)
                {
                   
                    return BadRequest();
                }
                var isExists = await _bookRepository.isExists(id);
                if (!isExists)
                {
                   
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    
                    return BadRequest(ModelState);
                }
                var book = _mapper.Map<Book>(bookDTO);
                var isSuccess = await _bookRepository.Update(book);
                if (!isSuccess)
                {
                    return InternalError($"{location}: Update failed for record with id: {id}");
                }
               
                return NoContent();
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }

        private string GetControllerActionNames()
        {
            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;

            return $"{controller} - {action}";
        }

        private ObjectResult InternalError(string message)
        {
            
            return StatusCode(500, "Something went wrong. Please contact the Administrator");
        }

    }
}