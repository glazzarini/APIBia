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
            CreateMap<LoginEntity, LoginResponse>()
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.LoginId))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.UserLogin, opt => opt.MapFrom(src => src.UserLogin))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => src.CreateDate))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Ativo));
        }
    }
}
