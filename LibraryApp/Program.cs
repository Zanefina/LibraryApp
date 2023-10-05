
using AutoMapper;
using FluentValidation;
using LibraryApp.Data;
using LibraryApp.Interfaces;
using LibraryApp.Models;
using LibraryApp.Models.DTO;
using LibraryApp.Repos;
using LibraryApp.Validations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Errors.Model;
using System.Net;

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
            builder.Services.AddAutoMapper(typeof(MappingConfig));
            builder.Services.AddValidatorsFromAssemblyContaining<CreateBookDTO>();

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


            app.MapGet("/api/books", async (IBookRepository bookRepo) =>
            {
                try
                {
                    // Retrieve a collection of books from the repository
                    var books = await bookRepo.GetAllBooks();

                    // Map the repository's book objects to BookDTO objects
                    var bookDTOs = books.Select(book => new BookDTO
                    {
                        BookId = book.BookId,
                        Title = book.Title,
                        Author = book.Author,
                        PublishedOn = book.PublishedOn,
                        Genre = book.Genre,
                        Description = book.Description,
                        Availability = book.Availability
                    }).ToList();

                    // Return the list of BookDTO objects as the response
                    return Results.Ok(bookDTOs);
                }
                catch (NotFoundException)
                {
                    return Results.NotFound("No books found");
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            }).WithName("GetAllBooks");
            app.MapGet("api/books/{id:int}", async (IBookRepository bookRepo, int id) =>
            {
                var book = await bookRepo.GetBookbyId(id);
                if (book == null)
                {
                    return Results.NotFound("The book that you are searching for was not found");
                }
                return Results.Ok(book);
            }).WithName("GetBookByID");
          
            app.MapPost("/api/books", async (IBookRepository bookRepo, IMapper mapper, CreateBookDTO createBookDTO, CreateBookValidation validator) =>
            {
                try
                {
                    // Validate the input using the CreateBookValidation validator
                    var validationResult = validator.Validate(createBookDTO);

                    if (!validationResult.IsValid)
                    {
                        // Return a 400 Bad Request response with validation errors
                        return Results.BadRequest(validationResult.Errors);
                    }

                    // Use AutoMapper to map CreateBookDTO to Book
                    var newBook = mapper.Map<Book>(createBookDTO);

                    // Call the repository to create the new book
                    var createdBook = await bookRepo.CreateBook(newBook);

                    if (createdBook == null)
                    {
                        return Results.BadRequest("Unable to add the new book");
                    }

                    // Return a 201 Created response with the newly created book's details
                    return Results.Created($"/api/Books/{createdBook.BookId}", createdBook);
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            }).WithName("CreateBook").Accepts<CreateBookDTO>("application/json");


            app.MapDelete("/api/books/{bookId}", async (int bookId, IBookRepository bookRepo) =>
            {
                var book = await bookRepo.DeleteBook(bookId);
                if (book != null)
                {
                    return Results.Ok($"Book with Id: {book.BookId} is deleted");
                }
                return Results.NotFound("Unable to find the book");
            }).WithName("DeleteBook");

            app.MapPut("/api/books", async (IBookRepository bookRepo, IMapper mapper, UpdateBookDTO updateBookDTO, int bookId, IValidator<UpdateBookDTO> validator) =>
            {
                try
                {
                    // Validate the input using the UpdateBookValidation validator
                    var validationResult = validator.Validate(updateBookDTO);

                    if (!validationResult.IsValid)
                    {
                        // Return a 400 Bad Request response with validation errors
                        return Results.BadRequest(validationResult.Errors);
                    }

                    // Use AutoMapper to map UpdateBookDTO to Book
                    var bookToUpdate = mapper.Map<Book>(updateBookDTO);

                    // Set the BookId property to the provided bookId
                    bookToUpdate.BookId = bookId;

                    // Call the repository to update the book
                    var updatedBook = await bookRepo.UpdateBook(bookToUpdate, bookId);

                    if (updatedBook != null)
                    {
                        return Results.Ok($"Book with Id: {updatedBook.BookId} was updated");
                    }

                    return Results.NotFound("Unable to find the book");
                }
                catch (Exception)
                {
                    return Results.StatusCode(500);
                }
            }).WithName("UpdateBook").Accepts<UpdateBookDTO>("application/json");



            app.Run();
        }

    }
}