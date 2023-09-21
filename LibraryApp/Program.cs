
using LibraryApp.Data;
using LibraryApp.Interfaces;
using LibraryApp.Models;
using LibraryApp.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LibraryApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register services and database context
            builder.Services.AddScoped<IBookRepository, BookRepository>();
            builder.Services.AddDbContext<Context>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
            });


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapGet("/api/GetAllBooks", async (IBookRepository bookRepo) =>
            {
                try
                {
                    var result = await bookRepo.GetAllBooks();
                    return Results.Ok(result);
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            });
            app.MapGet("api/GetBookByID/{id:int}", async (IBookRepository bookRepo, int id) =>
            {
                var book = await bookRepo.GetBookbyId(id);
                if (book == null)
                {
                    return Results.NotFound("The book that you are searching for was not found");
                }
                return Results.Ok(book);
            });
            app.MapPost("/api/CreateBook", async (IBookRepository bookRepo, Book book) =>
            {
                var newBook = await bookRepo.CreateBook(book);
                if (newBook == null)
                {
                    return Results.BadRequest("Unable to add the new book");
                }
                return Results.Ok(newBook);
            });
            app.MapDelete("/api/DeleteBook/{bookId}", async (int bookId, IBookRepository bookRepo) =>
            {
                var book = await bookRepo.DeleteBook(bookId);
                if (book != null)
                {
                    return Results.Ok($"Book with Id: {book.BookId} is deleted");
                }
                return Results.NotFound("Unable to find the book");
            });
            app.MapPut("/api/UpdateBook", async (IBookRepository bookRepo, Book book, int bookId) =>
            {
                var booktoUpdate = await bookRepo.UpdateBook(book, bookId);
                if (booktoUpdate != null)
                {
                    return Results.Ok($"Book with Id: {booktoUpdate.BookId} was updated");
                }
                return Results.NotFound($"Unable to find book");
                
            });

            app.Run();
        }

    }
}