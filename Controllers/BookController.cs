using LibraryApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly LibraryDbContext _context;
        public BookController(LibraryDbContext context)
        {
            _context = context;
        }

        //CREATE new book
        [HttpPost]
        public async Task<ActionResult<Book>> PostNewBookToLibrary(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            //REST-ful
            return CreatedAtAction("GetBooks", new {id = book.Id}, book);
        }

        //READ books in library
        [HttpGet]
        public async Task <ActionResult<IEnumerable<Book>>> GetBooks()
        {
            // return await _context.Books.ToListAsync();

            //For soft delete, to only show not deleted books
            return await _context.Books.Where(b => b.IsDeleted == false).ToListAsync();
        }

        //Find book from id
        [HttpGet ("{id}")]
        public async Task<ActionResult<Book>> GetBookById(long id)
        {
            //For soft delete, to only show not deleted books
            var book = await _context.Books.Where(b => !b.IsDeleted).FirstOrDefaultAsync(t => t.Id == id);
            
            if(book == null)
            {
                return NotFound();
            }
            
            return book;
        }

        public class MarkAsBorrowedDto
        {
            public bool IsAvailable {get; set;}
        }

        //UPDATE 
        //Use Patch as we only want to change one thing
        [HttpPatch("{id}")]
        public async Task<IActionResult> MarkAsBorrowed(long id, [FromBody] MarkAsBorrowedDto borrowedDto)
        {
            var book = await _context.Books.FindAsync(id);

            if(book == null)
            {
                return NotFound();
            }

            book.IsAvailable = borrowedDto.IsAvailable;
            await _context.SaveChangesAsync();

            return Ok(book);
        }

        //DELETE
        //---------Soft delete---------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(long id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            book.IsDeleted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
