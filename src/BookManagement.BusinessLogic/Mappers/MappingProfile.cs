using AutoMapper;
using BookManagement.BusinessLogic.Models.Books;
using BookManagement.BusinessLogic.Models.Users;
using BookManagement.Domain.Entities;

namespace BookManagement.BusinessLogic.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserRegisterModel, User>().ReverseMap();
        CreateMap<User, UserResultModel>().ReverseMap();

        CreateMap<BookCreateModel, Book>().ReverseMap();
        CreateMap<BookUpdateModel, Book>().ReverseMap();
        CreateMap<Book, BookResultModel>().ReverseMap();
    }
}
