using AutoMapper;
using LibraryApp.Models;
using LibraryApp.Models.DTO;

namespace LibraryApp
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Book, CreateBookDTO>().ReverseMap();
            CreateMap<Book, BookDTO>().ReverseMap();
            CreateMap<Book, UpdateBookDTO>().ReverseMap();

        }
    }
}
