using AutoMapper;
using BookStore.Data;
using BookStore.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Mappings
{
    public class Maps : Profile
    {
        public Maps()
        {
            CreateMap<Author, AuthorDTO>().ReverseMap();
            CreateMap<Author, CreateAuthorDTO>().ReverseMap();
            CreateMap<Author, AuthorUpdateDTO>().ReverseMap();
            CreateMap<Book, BookDTO>().ReverseMap();
        }
    }
}
