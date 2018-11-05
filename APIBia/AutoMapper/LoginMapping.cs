using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using APIBia.Application.Entities;
using APIBia.Application.Contracts;

namespace APIBia.AutoMapper
{
    public class LoginMapping : Profile
    {
        public LoginMapping()
        {
            CreateMap<Login, LoginResponse>()
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.ID_LOGIN))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PASSWORD))
                .ForMember(dest => dest.UserLogin, opt => opt.MapFrom(src => src.USER_LOGIN))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NAME))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CREATE_DATE))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.ACTIVE));
        }
    }
}
