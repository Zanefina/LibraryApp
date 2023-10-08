using LibraryApp.Models;
using LibraryApp.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using WebLibrary.Models;
using WebLibrary.Services;

namespace WebLibrary.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            this._bookService = bookService;
        }
        public async Task<IActionResult> BookIndex()
        {
            
            List<BookDTO> bookList = new List<BookDTO>();

            var response = await _bookService.GetAllBooks<ResponsDto>();

            if (response != null && response.IsSuccess)
            {
                try
                {
                    bookList = JsonConvert.DeserializeObject<List<BookDTO>>(Convert.ToString(response.Result));
                }
                catch (Exception ex)
                {
                    
                    ModelState.AddModelError("", "Error occurred while processing the data.");
                    
                }
            }
            else
            {
                ModelState.AddModelError("", "Error occurred while fetching data from the service.");
                
            }

            return View(bookList);
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _bookService.GetBookById<ResponsDto>(id);
			if (response != null && response.IsSuccess)
			{
				BookDTO model = JsonConvert.DeserializeObject<BookDTO>(Convert.ToString(response.Result));
				return View(model);
			}
			return NotFound();
        }

        public async Task<IActionResult> CreateBook()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(BookDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _bookService.CreateBookAsync<ResponsDto>(model);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(BookIndex));
                }
            }
            return View();
        }

        public async Task<IActionResult> UpdateBook(int id)
        {
            var response = await _bookService.GetBookById<ResponsDto>(id);
            if (response != null && response.IsSuccess)
            {
                BookDTO model = JsonConvert.DeserializeObject<BookDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBook(BookDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _bookService.UpdateBookAsync<ResponsDto>(model);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(BookIndex));
                }
            }
            return View(model);
        }


        public async Task<IActionResult> DeleteBook(int id)
        {
            var response = await _bookService.GetBookById<ResponsDto>(id);
            if (response != null && response.IsSuccess)
            {
                BookDTO model = JsonConvert.DeserializeObject<BookDTO>(Convert.ToString(response.Result));
                return View(model);
            }

            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> DeleteBook(BookDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _bookService.DeleteBookAsync<ResponsDto>(model.BookId);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(BookIndex));
                }
            }
            return NotFound();
        }

    }

}

