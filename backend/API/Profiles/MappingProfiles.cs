using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Domain.Entities;

namespace API.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<ProductTypeDto, ProductType>().ReverseMap();
            CreateMap<StateDto, State>().ReverseMap();

            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<UserRegisterDto, User>().ReverseMap();
        }
    }
}