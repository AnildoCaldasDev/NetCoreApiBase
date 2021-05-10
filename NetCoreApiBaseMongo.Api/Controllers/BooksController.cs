using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreApiBaseMongo.Api.Models;
using NetCoreApiBaseMongo.Api.Services;

namespace NetCoreApiBaseMongo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public ActionResult<List<Book>> GetAll()
        {
            return _bookService.Get();
        }

        [HttpGet("id:length(24)", Name = "GetBook")]
        public ActionResult<Book> Get(string id)
        {
            var book = _bookService.Get(id);
            if (book == null)
            {
                return NotFound();
            }
            return book;
        }

        [HttpPost]
        public ActionResult<Book> Post(Book newBook)
        {
            _bookService.Create(newBook);
            return CreatedAtRoute("GetBook", new { id = newBook.Id.ToString() }, newBook);
        }


        [HttpPut("{id:length(24)}")]
        public ActionResult Update(string id, Book bookIn)
        {
            var book = _bookService.Get(id);
            if (book == null)
            {
                return NotFound();
            }

            _bookService.Update(id, bookIn);

            return Ok(book);
        }

        [HttpDelete("{id:length(24)}")]
        public ActionResult Delete(string id)
        {
            var book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _bookService.Remove(book);
            return Ok(book);
        }
    }
}
