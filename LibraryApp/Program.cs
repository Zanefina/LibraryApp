
using AutoMapper;
using FluentValidation;
using LibraryApp.Data;
using LibraryApp.Interfaces;
using LibraryApp.Models;
using LibraryApp.Models.DTO;
using LibraryApp.Repos;
using LibraryApp.Validations;
using Microsoft.AspNetCore.Mvc;
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

            //adding cors policy! 
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CORSPolicy",
                    builder =>
                    {
                        builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithOrigins("http://localhost:3001");//route of local react application
                    });
            });

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

            app.UseCors("CORSPolicy");

            app.UseAuthorization();


            app.MapGet("/api/books/", async ([FromServices] IBookRepository _bookRepo) =>
            {
                ApiResponse response = new ApiResponse();

                response.Result = await _bookRepo.GetAllBooks();
                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;

                return Results.Ok(response);
            }).WithName("GetAllBooks").Produces<ApiResponse>(200);

            app.MapGet("/api/books/{bookId:int}", async (int bookId,[FromServices] IBookRepository _bookRepo) =>
            {
                //// Creats a respons object
                //ApiResponse response = new ApiResponse();
                //var book = await _bookRepo.GetBookbyId(bookId);

                //if (book != null)
                //{
                //    var bookDTO = _mapper.Map<IEnumerable<BookDTO>>(book);
                //    response.Result = bookDTO;
                //    response.IsSuccess = true;
                //    response.StatusCode = System.Net.HttpStatusCode.OK;
                //    return Results.Ok(response);
                //}
                //response.ErrorMessages.Add("No match was found!");
                //return Results.NotFound(response);
                // Creats a respons object
                ApiResponse response = new ApiResponse();

                // Add the wanted data to the respons result
                response.Result = await _bookRepo.GetBookbyId(bookId);

                // If data was not found 
                if (response.Result == null)
                {
                    response.IsSuccess = false;
                    response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    response.ErrorMessages.Add($"No book with Id {bookId} found!");
                    return Results.NotFound(response);
                }

                // if data was found
                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;

                return Results.Ok(response);
            }).WithName("GetBookById").Produces<ApiResponse>(200);

            app.MapPost("/api/books/",
            async (
            [FromServices] IValidator<CreateBookDTO> validator,
            [FromServices] IMapper _mapper, 
            [FromBody] CreateBookDTO CBookDTO,
            [FromServices] IBookRepository _bookRepo) =>
            {
                ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };

                // Validates the input object depending on the rule set in the validate class
                var validateResult = await validator.ValidateAsync(CBookDTO);
                if (!validateResult.IsValid)
                {
                    foreach (var error in validateResult.Errors.ToList())
                    {
                        response.ErrorMessages.Add(error.ToString());
                    }
                    return Results.BadRequest(response);
                }

                // Maps the DTO to the real book object
                Book book = _mapper.Map<Book>(CBookDTO);
                book.PublishedOn = DateTime.Now;
                book.Availability = true;

                response.Result = await _bookRepo.CreateBook(book);

                if (response.Result == null)
                {
                    response.ErrorMessages.Add($"A book with the title '{book.Title}' already exists");
                    return Results.BadRequest(response);
                }

              
                response.Result = _mapper.Map<BookDTO>(book);
                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.Created;
                return Results.Ok(response);
            }).WithName("CreateBook").Accepts<CreateBookDTO>("application/json").Produces<ApiResponse>(201).Produces(400);



            app.MapDelete("/api/books/{bookId:int}", async (int bookId, [FromServices] IBookRepository _bookRepo) =>
            {
                //ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.NotFound };

                //response.Result = await _bookRepo.DeleteBook(bookId);
                //if (response.Result == null)
                //{
                //    response.ErrorMessages.Add($"No book with ID {bookId} exists");
                //    return Results.NotFound(response);
                //}

                //response.IsSuccess = true;
                //response.StatusCode = System.Net.HttpStatusCode.NoContent;
                //return Results.Ok(response);
                ApiResponse response = new() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };
                try
                {
                    var bookToDelete = await _bookRepo.DeleteBook(bookId);
                    if (bookToDelete != null)
                    {
                        response.IsSuccess = true;
                        response.StatusCode = System.Net.HttpStatusCode.OK;
                        response.Result = bookToDelete;
                        return Results.Ok(response);
                    }
                    response.ErrorMessages.Add("invalid ID");
                    response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    return Results.NotFound(response);
                }
                catch (Exception e)
                {

                    return Results.BadRequest(e);
                }


            }).WithName("DeleteBook");

            app.MapPut("/api/books/",
            async (
            [FromServices] IValidator<UpdateBookDTO> validator,
            [FromServices] IMapper _mapper,
            [FromBody] UpdateBookDTO UBookDTO,
            [FromServices] IBookRepository _bookRepo) =>
            {
                ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = System.Net.HttpStatusCode.BadRequest };

                // Validates the input object depending on the rule set in the validate class
                var validateResult = await validator.ValidateAsync(UBookDTO);
                if (!validateResult.IsValid)
                {
                    foreach (var error in validateResult.Errors.ToList())
                    {
                        response.ErrorMessages.Add(error.ToString());
                    }
                    return Results.BadRequest(response);
                }

                Book book = _mapper.Map<Book>(UBookDTO);

                // Finds the book with the id we want to change
                response.Result = await _bookRepo.UpdateBook(book);

                // if it does not exists
                if (response.Result == null)
                {
                    response.ErrorMessages.Add($"No book with id {UBookDTO.BookId} exists");
                    response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    return Results.NotFound(response);
                }

               
                response.Result = _mapper.Map<BookDTO>(book);
                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;

                return Results.Ok(response);
            }).WithName("UpdateBook").Accepts<UpdateBookDTO>("Application/json").Produces<ApiResponse>(200).Produces(400);


            app.Run();
        }

    }
}