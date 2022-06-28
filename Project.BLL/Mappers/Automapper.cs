using AutoMapper;
using Project.DTO.DTOs.AuthorDTOs;
using Project.DTO.DTOs.BookDTOs;
using Project.DTO.DTOs.CategoryDTOs;
using Project.DTO.DTOs.CustomLoggingDTOs;
using Project.DTO.DTOs.UserDTOs;
using Project.Entity.Entities;

namespace Project.BLL.Mappers;

public class Automapper : Profile
{
    public Automapper()
    {
        CreateMap<UserToAddDTO, User>();
        CreateMap<UserToUpdateDTO, User>();
        CreateMap<User, UserToListDTO>();

        CreateMap<Category, CategoryToListDTO>();

        CreateMap<Author, AuthorToListDTO>();
        CreateMap<AuthorToAddDTO, Author>();

        CreateMap<ResponseLogDTO, ResponseLog>();
        CreateMap<RequestLogDTO, RequestLog>();

        CreateMap<Book, BookToListDTO>()
            .ForMember(dest => dest.PublishedAt, opt => opt.MapFrom(src => src.PublishedAt.ToString("dd/MM/yyyy")));

        CreateMap<BookToAddDTO, Book>();
    }
}